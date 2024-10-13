using Application.UseCases.V1.OrderDelivered.PostOrderDelivered.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderDelivered.PostOrderDelivered;

public class PostOrderDeliveredValidationUseCase : IPostOrderDeliveredUseCase
{
    private IOutputPort<Domain.Entities.OrderDelivered> _outputPort;
    private readonly IPostOrderDeliveredUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PostOrderDeliveredValidationUseCase(
        IPostOrderDeliveredUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.OrderDelivered orderDelivered)
    {
        if (orderDelivered.Invalid())
        {
            var listNotification = orderDelivered.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(orderDelivered);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.OrderDelivered> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}