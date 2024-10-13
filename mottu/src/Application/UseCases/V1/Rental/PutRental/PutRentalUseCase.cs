using Application.Services.Interfaces;
using Application.UseCases.V1.Rental.PutRental.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Rental.PutRental;

public class PutRentalUseCase : IPutRentalUseCase
{
    private IOutputPort<Domain.Entities.Rental> _outputPort;
    private IUnitOfWork _unitOfWork;
    private IRentalRepository _repository;
    private IRentalService _rentalService;
    private readonly NotificationHelper _notificationHelper;

    public PutRentalUseCase(
        IUnitOfWork unitOfWork,
        IRentalRepository repository,
        IRentalService rentalService,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _rentalService = rentalService;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.Rental rental)
    {
        var result =
                await _repository?.Where(c => c.Id == rental.Id)
                                 ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.RentalNotExist);

            _outputPort.Error();
        }
        else
        {
            rental =
                await _rentalService.Init(_outputPort,
                                          _notificationHelper,
                                          rental,
                                          false);

            if (rental == null)
                return;

            rental.Map(result);

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

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Rental> outputPort)
        => _outputPort = outputPort;
}