using Application.UseCases.V1.PlanType.PostPlanType.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.PlanType.PostPlanType;

public class PostPlanTypeValidationUseCase : IPostPlanTypeUseCase
{
    private IOutputPort<Domain.Entities.PlanType> _outputPort;
    private readonly IPostPlanTypeUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PostPlanTypeValidationUseCase(
        IPostPlanTypeUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.PlanType planType)
    {
        if (planType.Invalid())
        {
            var listNotification = planType.GetListNotification();

            foreach (var notification in listNotification)
                _notificationHelper.Add(SystemConst.Error, notification.ErrorMessage);

            _outputPort.Error();

            return;
        }

        try
        {
            await _useCase.Execute(planType);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.PlanType> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}