using Application.UseCases.V1.User.PostUser.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.User.PostUser;

public class PostUserUseCase : IPostUserUseCase
{
    private IOutputPort<Domain.Entities.User> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PostUserUseCase(
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
           await _repository?.Where(c => c.Name.ToUpper().Trim().Contains(normalizedName) ||
                                         c.Email.ToUpper().Trim().Contains(normalizedEmail))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.UserExist);

            _outputPort.Error();
        }
        else if (user.Id == 0)
        {
            user.SetDateCreated();

            await _repository.Add(user)
                             .ConfigureAwait(false);

            var response =
                await _unitOfWork.Save()
                                 .ConfigureAwait(false);

            if (!string.IsNullOrEmpty(response))
            {
                _notificationHelper.Add(SystemConst.Error, response);

                _outputPort.Error();
            }
            else
                _outputPort.Ok(user);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.User> outputPort)
        => _outputPort = outputPort;
}