using Application.UseCases.V1.OrderAccepted.PostOrderAccepted.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderAccepted.PostOrderAccepted;

public class PostOrderAcceptedValidationUseCase : IPostOrderAcceptedUseCase
{
    private IOutputPort<Domain.Entities.OrderAccepted> _outputPort;
    private readonly IPostOrderAcceptedUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PostOrderAcceptedValidationUseCase(
        IPostOrderAcceptedUseCase useCase,
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