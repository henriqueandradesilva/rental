using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Plan.Response;

public class PutPlanResponse : BaseResponse
{
    public PutPlanResponse()
    {

    }

    public PutPlanResponse(
        Domain.Entities.Plan plan)
    {
        Id = plan.Id;
        DataCriacao = plan.DateCreated;
        DataAlteracao = plan.DateUpdated;
    }
}