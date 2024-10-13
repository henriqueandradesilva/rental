using Application.UseCases.V1.Order.GetListAllOrder.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Order.GetListAllOrder;

public class GetListAllOrderUseCase : IGetListAllOrderUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.Order>> _outputPort;
    private IOrderRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllOrderUseCase(
        IOrderRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute()
    {
        var result =
            await _repository.GetAllWithIncludes(c => c.Status,
                                                 c => c.Accepted,
                                                 c => c.Delivered);

        if (result == null || result.Count == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.OrderNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Order>> outputPort)
        => _outputPort = outputPort;
}