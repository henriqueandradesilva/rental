using Application.UseCases.V1.Driver.GetListAllDriver.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.GetListAllDriver;

public class GetListAllDriverUseCase : IGetListAllDriverUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.Driver>> _outputPort;
    private readonly IDriverRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllDriverUseCase(
        IDriverRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute()
    {
        var result =
            await _repository.GetAllWithIncludes(c => c.User);

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.DriverNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Driver>> outputPort)
        => _outputPort = outputPort;
}