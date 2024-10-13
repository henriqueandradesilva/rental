using Application.UseCases.V1.OrderAccepted.GetOrderAcceptedById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderAccepted.GetOrderAcceptedById;

public class GetOrderAcceptedByIdUseCase : IGetOrderAcceptedByIdUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.OrderAccepted> _outputPort;
    private readonly IOrderAcceptedRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetOrderAcceptedByIdUseCase(
        IOrderAcceptedRepository repository,
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
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.OrderAcceptedNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.OrderAccepted> outputPort)
        => _outputPort = outputPort;
}