using Application.UseCases.V1.UserRole.GetUserRoleById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.UserRole.GetUserRoleById;

public class GetUserRoleByIdUseCase : IGetUserRoleByIdUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.UserRole> _outputPort;
    private readonly IUserRoleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetUserRoleByIdUseCase(
        IUserRoleRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        long id)
    {
        var result =
            await _repository?.Where(c => c.Id == id)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.UserRoleNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.UserRole> outputPort)
        => _outputPort = outputPort;
}