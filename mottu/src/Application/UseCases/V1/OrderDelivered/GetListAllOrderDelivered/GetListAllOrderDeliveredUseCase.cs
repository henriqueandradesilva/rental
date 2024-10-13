using Application.UseCases.V1.OrderDelivered.GetListAllOrderDelivered.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderDelivered.GetListAllOrderDelivered;

public class GetListAllOrderDeliveredUseCase : IGetListAllOrderDeliveredUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.OrderDelivered>> _outputPort;
    private readonly IOrderDeliveredRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllOrderDeliveredUseCase(
        IOrderDeliveredRepository repository,
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
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.OrderDeliveredNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.OrderDelivered>> outputPort)
        => _outputPort = outputPort;
}