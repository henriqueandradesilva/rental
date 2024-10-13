using Application.UseCases.V1.UserRole.GetListAllUserRole.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.UserRole.GetListAllUserRole;

public class GetListAllUserRoleUseCase : IGetListAllUserRoleUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.UserRole>> _outputPort;
    private readonly IUserRoleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllUserRoleUseCase(
        IUserRoleRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute()
    {
        var result =
             await _repository.GetAll();

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.UserRoleNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.UserRole>> outputPort)
        => _outputPort = outputPort;
}