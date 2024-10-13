using Application.UseCases.V1.ModelVehicle.PutModelVehicle.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.ModelVehicle.PutModelVehicle;

public class PutModelVehicleValidationUseCase : IPutModelVehicleUseCase
{
    private IOutputPort<Domain.Entities.ModelVehicle> _outputPort;
    private readonly IPutModelVehicleUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PutModelVehicleValidationUseCase(
        IPutModelVehicleUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.ModelVehicle modelVehicle)
    {
        if (modelVehicle.Invalid())
        {
            var listNotification = modelVehicle.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(modelVehicle);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.ModelVehicle> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}