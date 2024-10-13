using Application.UseCases.V1.Plan.GetListAllPlan.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Plan.GetListAllPlan;

public class GetListAllPlanUseCase : IGetListAllPlanUseCase
{
    private IOutputPortWithNotFound<List<Domain.Entities.Plan>> _outputPort;
    private readonly IPlanRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllPlanUseCase(
        IPlanRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute()
    {
        var result =
             await _repository.GetAllWithIncludes(c => c.PlanType);

        if (result == null || !result.Any())
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.PlanNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<List<Domain.Entities.Plan>> outputPort)
        => _outputPort = outputPort;
}