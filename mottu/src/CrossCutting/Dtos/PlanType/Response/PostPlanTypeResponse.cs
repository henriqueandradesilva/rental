using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.PlanType.Response;

public class PostPlanTypeResponse : BaseResponse
{
    public PostPlanTypeResponse()
    {

    }

    public PostPlanTypeResponse(
        Domain.Entities.PlanType planType)
    {
        Id = planType.Id;
        DataCriacao = planType.DateCreated;
        DataAlteracao = planType.DateUpdated;
    }
}