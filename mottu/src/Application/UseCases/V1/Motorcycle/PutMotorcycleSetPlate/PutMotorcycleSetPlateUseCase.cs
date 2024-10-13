using Application.UseCases.V1.Motorcycle.PutMotorcycleSetPlate.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.PutMotorcycleSetPlate;

public class PutMotorcycleSetPlateUseCase : IPutMotorcycleSetPlateUseCase
{
    private IOutputPort<Domain.Entities.Motorcycle> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMotorcycleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PutMotorcycleSetPlateUseCase(
        IUnitOfWork unitOfWork,
        IMotorcycleRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        long id,
        string plate)
    {
        var normalizedPlate = plate?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Id != id &&
                                         c.Plate.ToUpper().Trim().Contains(normalizedPlate))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.MotorcycleExist);

            _outputPort.Error();
        }
        else
        {
            result =
                await _repository?.Where(c => c.Id == id)
                                 ?.FirstOrDefaultAsync();

            if (result == null)
            {
                _notificationHelper.Add(SystemConst.NotFound, MessageConst.MotorcycleNotExist);

                _outputPort.Error();
            }
            else
            {
                result.SetPlate(plate);

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