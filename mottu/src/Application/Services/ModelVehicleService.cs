using Application.Services.Interfaces;
using Application.UseCases.V1.ModelVehicle.PostModelVehicle.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services;

public class ModelVehicleService : IModelVehicleService
{
    private readonly IPostModelVehicleUseCase _postModelVehicleUseCase;
    private readonly IModelVehicleRepository _modelVehicleRepository;

    public ModelVehicleService(
        IPostModelVehicleUseCase postModelVehicleUseCase,
        IModelVehicleRepository modelVehicleRepository)
    {
        _postModelVehicleUseCase = postModelVehicleUseCase;
        _modelVehicleRepository = modelVehicleRepository;
    }

    public async Task<long> CheckModelVehicleExist(
        IOutputPort<Motorcycle> outputPort,
        NotificationHelper notificationHelper,
        string modelVehicleDescription)
    {
        long modelVehicleId = 0;

        var normalizedModelVehicle =
            modelVehicleDescription?.NormalizeString();

        var result =
           await _modelVehicleRepository?.Where(c => c.Description.ToUpper().Trim().Contains(normalizedModelVehicle))
                                        ?.FirstOrDefaultAsync();

        if (result != null)
        {
            modelVehicleId = result.Id;

            return modelVehicleId;
        }

        return await CreateModelVehicle(outputPort, notificationHelper, modelVehicleDescription);
    }

    private async Task<long> CreateModelVehicle(
        IOutputPort<Motorcycle> outputPort,
        NotificationHelper notificationHelper,
        string modelVehicleDescription)
    {
        var modelVehicle =
            new ModelVehicle(0, modelVehicleDescription);

        var modelVehicleOutputPort =
            new OutputPortService<ModelVehicle>(notificationHelper);

        _postModelVehicleUseCase.SetOutputPort(modelVehicleOutputPort);

        await _postModelVehicleUseCase.Execute(modelVehicle);

        if (modelVehicleOutputPort.Errors.Any())
        {
            if (!notificationHelper.HasMessage)
            {
                foreach (var error in modelVehicleOutputPort.Errors)
                    notificationHelper.Add(SystemConst.Error, error);
            }

            outputPort.Error();
        }

        return modelVehicleOutputPort.Result?.Id ?? 0;
    }
}