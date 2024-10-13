using Application.UseCases.V1.OrderDelivered.GetOrderDeliveredById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderDelivered.GetOrderDeliveredById;

public class GetOrderDeliveredByIdUseCase : IGetOrderDeliveredByIdUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.OrderDelivered> _outputPort;
    private readonly IOrderDeliveredRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetOrderDeliveredByIdUseCase(
        IOrderDeliveredRepository repository,
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
                             ?.Include(c => c.Driver)
                             ?.Include(c => c.Order)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.OrderDeliveredNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.OrderDelivered> outputPort)
        => _outputPort = outputPort;
}