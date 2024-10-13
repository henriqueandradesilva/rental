using Application.UseCases.V1.OrderAccepted.PutOrderAccepted.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderAccepted.PutOrderAccepted;

public class PutOrderAcceptedValidationUseCase : IPutOrderAcceptedUseCase
{
    private IOutputPort<Domain.Entities.OrderAccepted> _outputPort;
    private readonly IPutOrderAcceptedUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PutOrderAcceptedValidationUseCase(
        IPutOrderAcceptedUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.OrderAccepted orderAccepted)
    {
        if (orderAccepted.Invalid())
        {
            var listNotification = orderAccepted.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(orderAccepted);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.OrderAccepted> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}