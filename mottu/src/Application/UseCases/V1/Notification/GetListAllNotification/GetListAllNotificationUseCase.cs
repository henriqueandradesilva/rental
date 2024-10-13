using Application.UseCases.V1.Notification.GetListAllNotification.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Notification.GetListAllNotification;

public class GetListAllNotificationUseCase : IGetListAllNotificationUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.Notification>> _outputPort;
    private readonly INotificationRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllNotificationUseCase(
        INotificationRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute()
    {
        var result =
             await _repository.GetAllWithIncludes(c => c.Order);

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.NotificationNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Notification>> outputPort)
        => _outputPort = outputPort;
}