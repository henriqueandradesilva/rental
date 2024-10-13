using Application.Services.Interfaces;
using Application.UseCases.V1.Motorcycle.PostMotorcycle.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.PostMotorcycle;

public class PostMotorcycleUseCase : IPostMotorcycleUseCase
{
    private IOutputPort<Domain.Entities.Motorcycle> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMotorcycleRepository _repository;
    private readonly IModelVehicleService _modelVehicleService;
    private readonly IRabbitMqService _rabbitMqService;
    private readonly NotificationHelper _notificationHelper;

    public PostMotorcycleUseCase(
        IUnitOfWork unitOfWork,
        IMotorcycleRepository repository,
        IModelVehicleService modelVehicleService,
        IRabbitMqService rabbitMqService,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _modelVehicleService = modelVehicleService;
        _rabbitMqService = rabbitMqService;
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
            await _modelVehicleService.CheckModelVehicleExist(_outputPort, _notificationHelper, modelVehicleDescription);

        if (modelVehicleId == 0)
            return;

        var normalizedPlate = motorcycle.Plate?.NormalizeString();

        var normalizedIdentifier = motorcycle.Identifier?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Plate.ToUpper().Trim().Contains(normalizedPlate) ||
                                         c.Identifier.ToUpper().Trim().Contains(normalizedIdentifier))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.MotorcycleExist);

            _outputPort.Error();

            return;
        }

        if (motorcycle.Id == 0)
        {
            motorcycle.SetDateCreated();

            motorcycle.SetModelVehicleId(modelVehicleId);

            await _repository.Add(motorcycle)
                             .ConfigureAwait(false);

            var response =
                await _unitOfWork.Save()
                                 .ConfigureAwait(false);

            if (!string.IsNullOrEmpty(response))
            {
                _notificationHelper.Add(SystemConst.Error, response);

                _outputPort.Error();

                return;
            }

            _rabbitMqService.SendMessage(motorcycle, SystemConst.MotorcycleEventCreatedQueue);

            _outputPort.Ok(motorcycle);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Motorcycle> outputPort)
        => _outputPort = outputPort;
}
