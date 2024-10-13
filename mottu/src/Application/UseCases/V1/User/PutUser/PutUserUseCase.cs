using Application.UseCases.V1.User.PutUser.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.User.PutUser;

public class PutUserUseCase : IPutUserUseCase
{
    private IOutputPort<Domain.Entities.User> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PutUserUseCase(
        IUnitOfWork unitOfWork,
        IUserRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.User user)
    {
        var normalizedName = user.Name?.NormalizeString();

        var normalizedEmail = user.Email?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Id != user.Id &&
                                         (
                                           c.Name.ToUpper().Trim().Contains(normalizedName) ||
                                           c.Email.ToUpper().Trim().Contains(normalizedEmail)
                                         ))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.UserExist);

            _outputPort.Error();
        }
        else
        {
            result =
                await _repository?.Where(c => c.Id == user.Id)
                                 ?.FirstOrDefaultAsync();

            if (result == null)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.UserNotExist);

                _outputPort.Error();
            }
            else
            {
                user.Map(result);

                result.SetDateUpdated();

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
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.User> outputPort)
        => _outputPort = outputPort;
}