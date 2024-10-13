using Application.UseCases.V1.UserRole.PostUserRole.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.UserRole.PostUserRole;

public class PostUserRoleUseCase : IPostUserRoleUseCase
{
    private IOutputPort<Domain.Entities.UserRole> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRoleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PostUserRoleUseCase(
        IUnitOfWork unitOfWork,
        IUserRoleRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.UserRole userRole)
    {
        var normalizedDescription = userRole.Description?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Description.ToUpper().Trim().Contains(normalizedDescription))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.UserRoleExist);

            _outputPort.Error();
        }
        else if (userRole.Id == 0)
        {
            userRole.SetDateCreated();

            await _repository.Add(userRole)
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
                _outputPort.Ok(userRole);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.UserRole> outputPort)
        => _outputPort = outputPort;
}