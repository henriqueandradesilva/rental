using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.Services.Interfaces;

public interface IDriverService
{
    Task<bool> SetCnhImage(
        IOutputPort<Domain.Entities.Driver> outputPort,
        NotificationHelper notificationHelper,
        long driverId,
        string cnhBase64);
}