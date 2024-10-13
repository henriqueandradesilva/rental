using Application.UseCases.V1.Plan.DeletePlan.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Plan.DeletePlan;

public class DeletePlanUseCase : IDeletePlanUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.Plan> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlanRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public DeletePlanUseCase(
        IUnitOfWork unitOfWork,
        IPlanRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
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
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.UserRoleNotExist);

            _outputPort.NotFound();
        }
        else
        {
            if (result.Id == SystemConst.PlanSevenDays ||
                result.Id == SystemConst.PlanFifteenDays ||
                result.Id == SystemConst.PlanThirtyDays ||
                result.Id == SystemConst.PlanFortyFiveDays ||
                result.Id == SystemConst.PlanFiftyDays)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

                _outputPort.Error();

                return;
            }

            _repository.Delete(result);

            var response =
                await _unitOfWork.Save()
                                 .ConfigureAwait(false);

            if (!string.IsNullOrEmpty(response))
            {
                _notificationHelper.Add(SystemConst.Error, response);

                _outputPort.Error();
            }
            else
                _outputPort.Ok(result);
        }
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Plan> outputPort)
        => _outputPort = outputPort;
}