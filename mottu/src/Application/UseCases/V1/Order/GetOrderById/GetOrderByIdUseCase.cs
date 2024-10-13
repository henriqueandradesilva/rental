using Application.UseCases.V1.Order.GetOrderById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Order.GetOrderById;

public class GetOrderByIdUseCase : IGetOrderByIdUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.Order> _outputPort;
    private IOrderRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetOrderByIdUseCase(
        IOrderRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        long id)
    {
        var result =
            await _repository?.Where(c => c.Id == id)
                             ?.Include(c => c.Status)
                             ?.Include(c => c.Accepted)
                             ?.Include(c => c.Delivered)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.OrderNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Order> outputPort)
        => _outputPort = outputPort;
}