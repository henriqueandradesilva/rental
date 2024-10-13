using Application.UseCases.V1.ModelVehicle.DeleteModelVehicle.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.ModelVehicle.DeleteModelVehicle;

public class DeleteModelVehicleUseCase : IDeleteModelVehicleUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.ModelVehicle> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IModelVehicleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public DeleteModelVehicleUseCase(
        IUnitOfWork unitOfWork,
        IModelVehicleRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        long id)
    {
        var result =
            await _repository?.Where(c => c.Id == id)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.ModelVehicleNotExist);

            _outputPort.NotFound();
        }
        else
        {
            _repository.Delete(result);

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

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.ModelVehicle> outputPort)
        => _outputPort = outputPort;
}