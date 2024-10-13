using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Services.Interfaces;

public interface IOrderAcceptedService
{
    Task<bool> InitOrder(
        IOutputPort<OrderAccepted> outputPort,
        NotificationHelper notificationHelper,
        long orderId);

    Task<bool> InitDriver(
          IOutputPort<OrderAccepted> outputPort,
          NotificationHelper notificationHelper,
          long driverId);
}