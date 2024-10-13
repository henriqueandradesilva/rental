using Application.UseCases.V1.OrderStatus.GetOrderStatusById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderStatus.GetOrderStatusById;

public class GetOrderStatusByIdUseCase : IGetOrderStatusByIdUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.OrderStatus> _outputPort;
    private readonly IOrderStatusRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetOrderStatusByIdUseCase(
        IOrderStatusRepository repository,
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
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.OrderStatusNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.OrderStatus> outputPort)
        => _outputPort = outputPort;
}