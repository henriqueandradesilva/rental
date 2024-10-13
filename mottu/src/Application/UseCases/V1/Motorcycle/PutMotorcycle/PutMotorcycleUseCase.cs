using Application.Services.Interfaces;
using Application.UseCases.V1.Motorcycle.PutMotorcycle.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.PutMotorcycle;

public class PutMotorcycleUseCase : IPutMotorcycleUseCase
{
    private IOutputPort<Domain.Entities.Motorcycle> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMotorcycleRepository _repository;
    private readonly IModelVehicleService _modelVehicleService;
    private readonly NotificationHelper _notificationHelper;

    public PutMotorcycleUseCase(
        IUnitOfWork unitOfWork,
        IMotorcycleRepository repository,
        IModelVehicleService modelVehicleService,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _modelVehicleService = modelVehicleService;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.Motorcycle motorcycle,
        string modelVehicleDescription)
    {
        if (string.IsNullOrEmpty(modelVehicleDescription))
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.MotorcycleModelVehicleIdRequired);

            _outputPort.Error();

            return;
        }

        var modelVehicleId =
            await _modelVehicleService.CheckModelVehicleExist(_outputPort,
                                                              _notificationHelper,
                                                              modelVehicleDescription);

        if (modelVehicleId == 0)
            return;

        var normalizedPlate = motorcycle.Plate?.NormalizeString();

        var normalizedIdentifier = motorcycle.Identifier?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Id != motorcycle.Id &&
                                         (
                                          c.Plate.ToUpper().Trim().Contains(normalizedPlate) ||
                                          c.Identifier.ToUpper().Trim().Contains(normalizedIdentifier)
                                         ))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.MotorcycleExist);

            _outputPort.Error();
        }
        else
        {
            result =
                await _repository?.Where(c => c.Id == motorcycle.Id)
                                 ?.FirstOrDefaultAsync();

            if (result == null)
            {
                _notificationHelper.Add(SystemConst.NotFound, MessageConst.MotorcycleNotExist);

                _outputPort.Error();
            }
            else
            {
                motorcycle.Map(result);

                result.SetDateUpdated();

                result.SetModelVehicleId(modelVehicleId);

                _repository.Update(result);

                var response =
                    await _unitOfWork.Save()
                                     .ConfigureAwait(false);

                if (!string.IsNullOrEmpty(response))
                {
                    _notificationHelper.Add(SystemConst.Error, response);

                    _outputPort.Error();
                }
                else
                    _outputPort.Ok(result);
            }
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Motorcycle> outputPort)
        => _outputPort = outputPort;
}