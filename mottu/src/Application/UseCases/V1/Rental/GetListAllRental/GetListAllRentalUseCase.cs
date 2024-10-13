using Application.UseCases.V1.Rental.GetListAllRental.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Rental.GetListAllRental;

public class GetListAllRentalUseCase : IGetListAllRentalUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.Rental>> _outputPort;
    private IRentalRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllRentalUseCase(
        IRentalRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute()
    {
        var result =
            await _repository.GetAllWithIncludes(c => c.Motorcycle,
                                                 c => c.Driver,
                                                 c => c.Plan);

        if (result == null || result.Count == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.RentalNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Rental>> outputPort)
        => _outputPort = outputPort;
}