using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System.Threading.Tasks;

namespace Application.Services.Interfaces;

public interface IModelVehicleService
{
    Task<long> CheckModelVehicleExist(
        IOutputPort<Domain.Entities.Motorcycle> outputPort,
        NotificationHelper notificationHelper,
        string modelVehicleDescription);
}