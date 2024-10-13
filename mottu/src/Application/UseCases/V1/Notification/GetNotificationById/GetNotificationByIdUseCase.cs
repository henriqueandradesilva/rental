using Application.UseCases.V1.Notification.GetNotificationById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Notification.GetNotificationById;

public class GetNotificationByIdUseCase : IGetNotificationByIdUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.Notification> _outputPort;
    private readonly INotificationRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetNotificationByIdUseCase(
        INotificationRepository repository,
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
                             ?.Include(c => c.Order)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.NotificationNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Notification> outputPort)
        => _outputPort = outputPort;
}