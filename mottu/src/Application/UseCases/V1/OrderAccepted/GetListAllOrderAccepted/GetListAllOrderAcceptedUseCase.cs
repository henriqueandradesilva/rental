using Application.UseCases.V1.OrderAccepted.GetListAllOrderAccepted.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.OrderAccepted.GetListAllOrderAccepted;

public class GetListAllOrderAcceptedUseCase : IGetListAllOrderAcceptedUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.OrderAccepted>> _outputPort;
    private readonly IOrderAcceptedRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllOrderAcceptedUseCase(
        IOrderAcceptedRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute()
    {
        var result =
             await _repository.GetAllWithIncludes(c => c.Order,
                                                  c => c.Driver);

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.OrderAcceptedNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.OrderAccepted>> outputPort)
        => _outputPort = outputPort;
}