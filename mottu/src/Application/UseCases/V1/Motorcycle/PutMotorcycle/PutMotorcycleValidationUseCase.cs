using Application.UseCases.V1.Motorcycle.PutMotorcycle.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.PutMotorcycle;

public class PutMotorcycleValidationUseCase : IPutMotorcycleUseCase
{
    private IOutputPort<Domain.Entities.Motorcycle> _outputPort;
    private readonly IPutMotorcycleUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PutMotorcycleValidationUseCase(
        IPutMotorcycleUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.Motorcycle motorcycle,
        string modelVehicleDescription)
    {
        if (motorcycle.Invalid())
        {
            var listNotification = motorcycle.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(motorcycle, modelVehicleDescription);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Motorcycle> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}