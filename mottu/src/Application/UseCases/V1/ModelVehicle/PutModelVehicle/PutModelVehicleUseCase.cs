using Application.UseCases.V1.ModelVehicle.PutModelVehicle.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.ModelVehicle.PutModelVehicle;

public class PutModelVehicleUseCase : IPutModelVehicleUseCase
{
    private IOutputPort<Domain.Entities.ModelVehicle> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IModelVehicleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PutModelVehicleUseCase(
        IUnitOfWork unitOfWork,
        IModelVehicleRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.ModelVehicle modelVehicle)
    {
        var normalizedDescription = modelVehicle.Description?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Id != modelVehicle.Id &&
                                         c.Description.ToUpper().Trim().Contains(normalizedDescription))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ModelVehicleExist);

            _outputPort.Error();
        }
        else
        {
            result =
                await _repository?.Where(c => c.Id == modelVehicle.Id)
                                 ?.FirstOrDefaultAsync();

            if (result == null)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.ModelVehicleNotExist);

                _outputPort.Error();
            }
            else
            {
                modelVehicle.Map(result);

                result.SetDateUpdated();

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
        IOutputPort<Domain.Entities.ModelVehicle> outputPort)
        => _outputPort = outputPort;
}