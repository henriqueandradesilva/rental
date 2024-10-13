using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Services.Interfaces;

public interface IRentalService
{
    Task<Rental> Init(
        IOutputPort<Rental> outputPort,
        NotificationHelper notificationHelper,
        Rental rental,
        bool checkMotorcycleIsRented);

    Task<Rental> Finish(
        IOutputPort<Rental> outputPort,
        NotificationHelper notificationHelper,
        Rental rental);
}