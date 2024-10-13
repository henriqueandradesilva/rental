using Application.UseCases.V1.ModelVehicle.GetListAllModelVehicle.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.ModelVehicle.GetListAllModelVehicle;

public class GetListAllModelVehicleUseCase : IGetListAllModelVehicleUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.ModelVehicle>> _outputPort;
    private readonly IModelVehicleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllModelVehicleUseCase(
        IModelVehicleRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute()
    {
        var result =
             await _repository.GetAll();

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.ModelVehicleNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.ModelVehicle>> outputPort)
        => _outputPort = outputPort;
}