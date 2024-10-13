using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Services.Interfaces;

public interface IOrderDeliveredService
{
    Task<bool> InitOrder(
        IOutputPort<OrderDelivered> outputPort,
        NotificationHelper notificationHelper,
        long orderId);

    Task<bool> InitDriver(
          IOutputPort<OrderDelivered> outputPort,
          NotificationHelper notificationHelper,
          long driverId);
}