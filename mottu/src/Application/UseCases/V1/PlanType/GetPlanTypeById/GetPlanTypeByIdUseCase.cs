using Application.UseCases.V1.PlanType.GetPlanTypeById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.PlanType.GetPlanTypeById;

public class GetPlanTypeByIdUseCase : IGetPlanTypeByIdUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.PlanType> _outputPort;
    private readonly IPlanTypeRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetPlanTypeByIdUseCase(
        IPlanTypeRepository repository,
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
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.PlanTypeNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.PlanType> outputPort)
        => _outputPort = outputPort;
}