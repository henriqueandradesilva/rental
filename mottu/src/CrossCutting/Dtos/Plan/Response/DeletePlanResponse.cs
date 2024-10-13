using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Plan.Response;

public class DeletePlanResponse : BaseDeleteResponse
{
    public DeletePlanResponse()
    {

    }

    public DeletePlanResponse(
        Domain.Entities.Plan plan)
    {
        Id = plan.Id;
    }
}