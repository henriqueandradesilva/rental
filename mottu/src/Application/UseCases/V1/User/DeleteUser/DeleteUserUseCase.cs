using Application.UseCases.V1.User.DeleteUser.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.User.DeleteUser;

public class DeleteUserUseCase : IDeleteUserUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.User> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public DeleteUserUseCase(
        IUnitOfWork unitOfWork,
        IUserRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        long id)
    {
        var result =
            await _repository?.Where(c => c.Id == id)
                             ?.Include(c => c.UserRole)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.UserNotExist);

            _outputPort.NotFound();
        }
        else
        {
            if (result.Id == SystemConst.UserAdminIdDefault)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

                _outputPort.Error();

                return;
            }

            _repository.Delete(result);

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
        IOutputPortWithNotFound<Domain.Entities.User> outputPort)
        => _outputPort = outputPort;
}