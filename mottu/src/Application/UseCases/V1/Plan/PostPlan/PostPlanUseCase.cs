using Application.UseCases.V1.Plan.PostPlan.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Plan.PostPlan;

public class PostPlanUseCase : IPostPlanUseCase
{
    private IOutputPort<Domain.Entities.Plan> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlanRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PostPlanUseCase(
        IUnitOfWork unitOfWork,
        IPlanRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.Plan plan)
    {
        var normalizedDescription = plan.Description?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Description.ToUpper().Trim().Contains(normalizedDescription))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.PlanExist);

            _outputPort.Error();
        }
        else if (plan.Id == 0)
        {
            plan.SetDateCreated();

            await _repository.Add(plan)
                             .ConfigureAwait(false);

            var response =
                await _unitOfWork.Save()
                                 .ConfigureAwait(false);

            if (!string.IsNullOrEmpty(response))
            {
                _notificationHelper.Add(SystemConst.Error, response);

                _outputPort.Error();
            }
            else
                _outputPort.Ok(plan);
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Plan> outputPort)
        => _outputPort = outputPort;
}