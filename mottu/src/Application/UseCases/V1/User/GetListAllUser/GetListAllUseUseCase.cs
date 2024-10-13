using Application.UseCases.V1.User.GetListAllUser.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.User.GetListAllUser;

public class GetListAllUserUseCase : IGetListAllUserUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.User>> _outputPort;
    private readonly IUserRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllUserUseCase(
        IUserRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute()
    {
        var result =
             await _repository.GetAllWithIncludes(c => c.UserRole,
                                                  c => c.Driver);

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.UserNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.User>> outputPort)
        => _outputPort = outputPort;
}