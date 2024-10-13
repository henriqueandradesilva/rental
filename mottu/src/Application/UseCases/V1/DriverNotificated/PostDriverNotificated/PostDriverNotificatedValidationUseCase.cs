using Application.UseCases.V1.DriverNotificated.PostDriverNotificated.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.DriverNotificated.PostDriverNotificated;

public class PostDriverNotificatedValidationUseCase : IPostDriverNotificatedUseCase
{
    private IOutputPort<Domain.Entities.DriverNotificated> _outputPort;
    private readonly IPostDriverNotificatedUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PostDriverNotificatedValidationUseCase(
        IPostDriverNotificatedUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.DriverNotificated driverNotificated)
    {
        if (driverNotificated.Invalid())
        {
            var listNotification = driverNotificated.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(driverNotificated);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.DriverNotificated> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}