using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.Services.Interfaces;

public interface IUserService
{
    Task<long> CheckUserExist(
        IOutputPort<Domain.Entities.Driver> outputPort,
        NotificationHelper notificationHelper,
        string userName,
        string cnh);

    Task<bool> SetDriverId(
        IOutputPort<Domain.Entities.Driver> outputPort,
        NotificationHelper notificationHelper,
        long userId,
        long driverId);
}