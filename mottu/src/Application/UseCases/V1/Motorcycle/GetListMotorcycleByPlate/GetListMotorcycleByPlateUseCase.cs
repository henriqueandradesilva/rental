using Application.UseCases.V1.Motorcycle.GetListMotorcycleByPlate.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.GetListMotorcycleByPlate;

public class GetListMotorcycleByPlateUseCase : IGetListMotorcycleByPlateUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.Motorcycle>> _outputPort;
    private readonly IMotorcycleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListMotorcycleByPlateUseCase(
        IMotorcycleRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        string plate)
    {
        if (string.IsNullOrWhiteSpace(plate))
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.MotorcyclePlateRequired);

            _outputPort.NotFound();

            return;
        }

        var normalizedPlate = plate?.NormalizeString();

        var result =
            await _repository?.Where(c => c.Plate.ToUpper().Trim().Contains(normalizedPlate))
                             ?.Include(c => c.ModelVehicle)
                             ?.ToListAsync();

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