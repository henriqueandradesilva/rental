using Application.UseCases.V1.Driver.PutDriver.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.PutDriver;

public class PutDriverValidationUseCase : IPutDriverUseCase
{
    private IOutputPort<Domain.Entities.Driver> _outputPort;
    private readonly IPutDriverUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PutDriverValidationUseCase(
        IPutDriverUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.Driver driver,
        string cnhBase64)
    {
        if (driver.Invalid())
        {
            var listNotification = driver.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(driver, cnhBase64);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Driver> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}