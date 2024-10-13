using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Services.Interfaces;

public interface IDriverNotificatedService
{
    Task<DriverNotificated> Init(
        IOutputPort<DriverNotificated> outputPort,
        NotificationHelper notificationHelper,
        DriverNotificated driverNotificated);
}