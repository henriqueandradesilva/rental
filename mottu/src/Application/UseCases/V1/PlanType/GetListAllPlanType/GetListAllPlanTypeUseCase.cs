using Application.UseCases.V1.PlanType.GetListAllPlanType.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.PlanType.GetListAllPlanType;

public class GetListAllPlanTypeUseCase : IGetListAllPlanTypeUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.PlanType>> _outputPort;
    private readonly IPlanTypeRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllPlanTypeUseCase(
        IPlanTypeRepository repository,
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
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.PlanTypeNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.PlanType>> outputPort)
        => _outputPort = outputPort;
}