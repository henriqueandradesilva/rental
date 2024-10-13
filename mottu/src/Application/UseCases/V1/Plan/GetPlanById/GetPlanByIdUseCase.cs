using Application.UseCases.V1.Plan.GetPlanById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Plan.GetPlanById;

public class GetPlanByIdUseCase : IGetPlanByIdUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.Plan> _outputPort;
    private readonly IPlanRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetPlanByIdUseCase(
        IPlanRepository repository,
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
                             ?.Include(c => c.PlanType)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.PlanNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Plan> outputPort)
        => _outputPort = outputPort;
}