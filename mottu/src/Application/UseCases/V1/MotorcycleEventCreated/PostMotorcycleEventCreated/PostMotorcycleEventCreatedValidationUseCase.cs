using Application.UseCases.V1.MotorcycleEventCreated.PostMotorcycleEventCreated.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.MotorcycleEventCreated.PostMotorcycleEventCreated;

public class PostMotorcycleEventCreatedValidationUseCase : IPostMotorcycleEventCreatedUseCase
{
    private IOutputPort<Domain.Entities.MotorcycleEventCreated> _outputPort;
    private readonly IPostMotorcycleEventCreatedUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PostMotorcycleEventCreatedValidationUseCase(
        IPostMotorcycleEventCreatedUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.MotorcycleEventCreated motorcycleEventCreated)
    {
        if (motorcycleEventCreated.Invalid())
        {
            var listNotification = motorcycleEventCreated.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(motorcycleEventCreated);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.MotorcycleEventCreated> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}