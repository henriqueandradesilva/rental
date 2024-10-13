using Application.UseCases.V1.Motorcycle.GetListAllMotorcycle.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.GetListAllMotorcycle;

public class GetListAllMotorcycleUseCase : IGetListAllMotorcycleUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.Motorcycle>> _outputPort;
    private readonly IMotorcycleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllMotorcycleUseCase(
        IMotorcycleRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute()
    {
        var result =
            await _repository.GetAllWithIncludes(c => c.ModelVehicle);

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.MotorcycleNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Motorcycle>> outputPort)
        => _outputPort = outputPort;
}