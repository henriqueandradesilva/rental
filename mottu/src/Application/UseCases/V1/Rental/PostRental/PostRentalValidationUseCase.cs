using Application.UseCases.V1.Rental.PostRental.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Rental.PostRental;

public class PostRentalValidationUseCase : IPostRentalUseCase
{
    private IOutputPort<Domain.Entities.Rental> _outputPort;
    private readonly IPostRentalUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PostRentalValidationUseCase(
        IPostRentalUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.Rental rental)
    {
        if (rental.Invalid())
        {
            var listNotification = rental.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(rental);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Rental> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}