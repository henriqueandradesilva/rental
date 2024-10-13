using Application.UseCases.V1.Plan.PostPlan.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Plan.PostPlan;

public class PostPlanValidationUseCase : IPostPlanUseCase
{
    private IOutputPort<Domain.Entities.Plan> _outputPort;
    private readonly IPostPlanUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PostPlanValidationUseCase(
        IPostPlanUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.Plan plan)
    {
        if (plan.Invalid())
        {
            var listNotification = plan.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(plan);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Plan> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}