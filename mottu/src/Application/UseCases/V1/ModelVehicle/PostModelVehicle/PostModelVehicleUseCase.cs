using Application.UseCases.V1.ModelVehicle.PostModelVehicle.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.ModelVehicle.PostModelVehicle;

public class PostModelVehicleUseCase : IPostModelVehicleUseCase
{
    private IOutputPort<Domain.Entities.ModelVehicle> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IModelVehicleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PostModelVehicleUseCase(
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
           await _repository?.Where(c => c.Description.ToUpper().Trim().Contains(normalizedDescription))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ModelVehicleExist);

            _outputPort.Error();
        }
        else if (modelVehicle.Id == 0)
        {
            modelVehicle.SetDateCreated();

            await _repository.Add(modelVehicle)
                             .ConfigureAwait(false);

            var response =
                await _unitOfWork.Save()
                                 .ConfigureAwait(false);

            if (!string.IsNullOrEmpty(response))
            {
                _notificationHelper.Add(SystemConst.Error, response);

                _outputPort.Error();
            }
            else
                _outputPort.Ok(modelVehicle);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.ModelVehicle> outputPort)
        => _outputPort = outputPort;
}