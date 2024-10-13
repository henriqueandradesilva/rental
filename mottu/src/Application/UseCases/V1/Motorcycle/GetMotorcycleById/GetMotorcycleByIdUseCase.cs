using Application.UseCases.V1.Motorcycle.GetMotorcycleById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.GetMotorcycleById;

public class GetMotorcycleByIdUseCase : IGetMotorcycleByIdUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.Motorcycle> _outputPort;
    private readonly IMotorcycleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetMotorcycleByIdUseCase(
        IMotorcycleRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        long id)
    {
        var result =
            await _repository?.Where(c => c.Id == id)
                             ?.Include(c => c.ModelVehicle)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.MotorcycleNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Motorcycle> outputPort)
        => _outputPort = outputPort;
}