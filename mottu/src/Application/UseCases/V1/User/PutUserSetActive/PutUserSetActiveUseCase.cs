using Application.UseCases.V1.User.PutUserSetActive.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.User.PutUserSetActive;

public class PutUserSetActiveUseCase : IPutUserSetActiveUseCase
{
    private IOutputPort<Domain.Entities.User> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PutUserSetActiveUseCase(
        IUnitOfWork unitOfWork,
        IUserRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        long id,
        bool active)
    {
        var result =
            await _repository?.Where(c => c.Id == id)
                             ?.Include(c => c.UserRole)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.UserNotExist);

            _outputPort.Error();
        }
        else
        {
            result.SetActive(active);

            _repository.Update(result);

            var response =
                await _unitOfWork.Save()
                                 .ConfigureAwait(false);

            if (!string.IsNullOrEmpty(response))
            {
                _notificationHelper.Add(SystemConst.Error, response);

                _outputPort.Error();
            }
            else
                _outputPort.Ok(result);
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.User> outputPort)
        => _outputPort = outputPort;
}