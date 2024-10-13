using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.PlanType.Response;

public class PutPlanTypeResponse : BaseResponse
{
    public PutPlanTypeResponse()
    {

    }

    public PutPlanTypeResponse(
        Domain.Entities.PlanType planType)
    {
        Id = planType.Id;
        DataCriacao = planType.DateCreated;
        DataAlteracao = planType.DateUpdated;
    }
}