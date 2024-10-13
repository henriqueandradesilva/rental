using Application.UseCases.V1.OrderStatus.PutOrderStatus.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderStatus.PutOrderStatus;

public class PutOrderStatusValidationUseCase : IPutOrderStatusUseCase
{
    private IOutputPort<Domain.Entities.OrderStatus> _outputPort;
    private readonly IPutOrderStatusUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PutOrderStatusValidationUseCase(
        IPutOrderStatusUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.OrderStatus orderStatus)
    {
        if (orderStatus.Invalid())
        {
            var listNotification = orderStatus.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(orderStatus);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.OrderStatus> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}