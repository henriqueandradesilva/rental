using Application.UseCases.V1.DriverNotificated.GetListAllDriverNotificated.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.DriverNotificated.GetListAllDriverNotificated;

public class GetListAllDriverNotificatedUseCase : IGetListAllDriverNotificatedUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.DriverNotificated>> _outputPort;
    private readonly IDriverNotificatedRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllDriverNotificatedUseCase(
        IDriverNotificatedRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute()
    {
        var result =
             await _repository.GetAllWithIncludes(c => c.Driver,
                                                  c => c.Notification);

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.DriverNotificatedNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.DriverNotificated>> outputPort)
        => _outputPort = outputPort;
}