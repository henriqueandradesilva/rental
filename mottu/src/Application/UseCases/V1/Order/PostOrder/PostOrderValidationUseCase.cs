using Application.UseCases.V1.Order.PostOrder.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Order.PostOrder;

public class PostOrderValidationUseCase : IPostOrderUseCase
{
    private IOutputPort<Domain.Entities.Order> _outputPort;
    private readonly IPostOrderUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PostOrderValidationUseCase(
        IPostOrderUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.Order order)
    {
        if (order.Invalid())
        {
            var listNotification = order.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(order);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Order> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}