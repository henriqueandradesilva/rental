using Application.UseCases.V1.Motorcycle.PutMotorcycleSetRented.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Motorcycle.PutMotorcycleSetRented;

public class PutMotorcycleSetRentedUseCase : IPutMotorcycleSetRentedUseCase
{
    private IOutputPort<Domain.Entities.Motorcycle> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMotorcycleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PutMotorcycleSetRentedUseCase(
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
        bool rented)
    {
        var result =
            await _repository?.Where(c => c.Id == id)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.MotorcycleNotExist);

            _outputPort.Error();
        }
        else
        {
            result.SetRented(rented);

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

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Motorcycle> outputPort)
        => _outputPort = outputPort;
}