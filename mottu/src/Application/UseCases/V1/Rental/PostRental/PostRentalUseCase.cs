using Application.Services.Interfaces;
using Application.UseCases.V1.Rental.PostRental.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Rental.PostRental;

public class PostRentalUseCase : IPostRentalUseCase
{
    private IOutputPort<Domain.Entities.Rental> _outputPort;
    private IUnitOfWork _unitOfWork;
    private IRentalRepository _repository;
    private IRentalService _rentalService;
    private readonly NotificationHelper _notificationHelper;

    public PostRentalUseCase(
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
        if (rental.Id == 0)
        {
            rental =
                await _rentalService.Init(_outputPort,
                                          _notificationHelper,
                                          rental,
                                          true);

            if (rental == null)
                return;

            rental.SetDateCreated();

            await _repository.Add(rental)
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
                _outputPort.Ok(rental);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Rental> outputPort)
        => _outputPort = outputPort;
}