using Application.UseCases.V1.OrderStatus.GetListAllOrderStatus.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderStatus.GetListAllOrderStatus;

public class GetListAllOrderStatusUseCase : IGetListAllOrderStatusUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.OrderStatus>> _outputPort;
    private readonly IOrderStatusRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllOrderStatusUseCase(
        IOrderStatusRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute()
    {
        var result =
             await _repository.GetAll();

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.OrderStatusNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.OrderStatus>> outputPort)
        => _outputPort = outputPort;
}