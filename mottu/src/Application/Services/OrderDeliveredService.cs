using Application.Services.Interfaces;
using Application.UseCases.V1.Driver.PutDriverSetDelivering.Interfaces;
using Application.UseCases.V1.Order.PutOrderSetStatus.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services;

public class OrderDeliveredService : IOrderDeliveredService
{
    private readonly IPutOrderSetStatusUseCase _putOrderSetStatusUseCase;
    private readonly IPutDriverSetDeliveringUseCase _putDriverSetDeliveringUseCase;
    private IOrderRepository _orderRepository;
    private IDriverRepository _driverRepository;

    public OrderDeliveredService(
        IPutOrderSetStatusUseCase putOrderSetStatusUseCase,
        IPutDriverSetDeliveringUseCase putDriverSetDeliveringUseCase,
        IOrderRepository orderRepository,
        IDriverRepository driverRepository)
    {
        _putOrderSetStatusUseCase = putOrderSetStatusUseCase;
        _putDriverSetDeliveringUseCase = putDriverSetDeliveringUseCase;
        _orderRepository = orderRepository;
        _driverRepository = driverRepository;
    }

    public async Task<bool> InitOrder(
        IOutputPort<OrderDelivered> outputPort,
        NotificationHelper notificationHelper,
        long orderId)
    {
        var order =
           await _orderRepository?.Where(c => c.Id == orderId)
                                 ?.FirstOrDefaultAsync();

        if (order == null)
        {
            notificationHelper.Add(SystemConst.Error, MessageConst.OrderNotExist);

            outputPort.Error();

            return false;
        }

        if (order.StatusId != SystemConst.OrderStatusAcceptedDefault)
        {
            notificationHelper.Add(SystemConst.Error, MessageConst.OrderStatusActionPermitted);

            outputPort.Error();

            return false;
        }

        var orderOutputPort =
            new OutputPortService<Order>(notificationHelper);

        _putOrderSetStatusUseCase.SetOutputPort(orderOutputPort);

        await _putOrderSetStatusUseCase.Execute(orderId, SystemConst.OrderStatusDeliveredDefault);

        if (orderOutputPort.Errors.Any())
        {
            if (!notificationHelper.HasMessage)
            {
                foreach (var error in orderOutputPort.Errors)
                    notificationHelper.Add(SystemConst.Error, error);
            }

            outputPort.Error();
        }

        return orderOutputPort?.Result?.Id > 0;
    }

    public async Task<bool> InitDriver(
        IOutputPort<OrderDelivered> outputPort,
        NotificationHelper notificationHelper,
        long driverId)
    {
        var driver =
           await _driverRepository?.Where(c => c.Id == driverId)
                                  ?.FirstOrDefaultAsync();

        if (driver == null)
        {
            notificationHelper.Add(SystemConst.Error, MessageConst.DriverNotExist);

            outputPort.Error();

            return false;
        }

        var driverOutputPort =
            new OutputPortService<Driver>(notificationHelper);

        _putDriverSetDeliveringUseCase.SetOutputPort(driverOutputPort);

        await _putDriverSetDeliveringUseCase.Execute(driverId, false);

        if (driverOutputPort.Errors.Any())
        {
            if (!notificationHelper.HasMessage)
            {
                foreach (var error in driverOutputPort.Errors)
                    notificationHelper.Add(SystemConst.Error, error);
            }

            outputPort.Error();
        }

        return driverOutputPort?.Result?.Id > 0;
    }
}