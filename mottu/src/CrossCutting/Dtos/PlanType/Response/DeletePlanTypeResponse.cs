using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.PlanType.Response;

public class DeletePlanTypeResponse : BaseDeleteResponse
{
    public DeletePlanTypeResponse()
    {

    }

    public DeletePlanTypeResponse(
        Domain.Entities.PlanType planType)
    {
        Id = planType.Id;
    }
}