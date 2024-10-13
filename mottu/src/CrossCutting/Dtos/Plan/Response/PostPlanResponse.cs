using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Plan.Response;

public class PostPlanResponse : BaseResponse
{
    public PostPlanResponse()
    {

    }

    public PostPlanResponse(
        Domain.Entities.Plan plan)
    {
        Id = plan.Id;
        DataCriacao = plan.DateCreated;
        DataAlteracao = plan.DateUpdated;
    }
}