using Application.UseCases.V1.Notification.PostNotification.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Notification.PostNotification;

public class PostNotificationValidationUseCase : IPostNotificationUseCase
{
    private IOutputPort<Domain.Entities.Notification> _outputPort;
    private readonly IPostNotificationUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PostNotificationValidationUseCase(
        IPostNotificationUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.Notification notification)
    {
        if (notification.Invalid())
        {
            var listNotification = notification.GetListNotification();

            foreach (var notificationFailure in listNotification)
                _notificationHelper.Add(SystemConst.Error, notificationFailure.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(notification);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Notification> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}