using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.PlanType.Response;

public class GetPlanTypeResponse : BaseResponse
{
    public string Descricao { get; set; }

    public GetPlanTypeResponse()
    {

    }

    public GetPlanTypeResponse GetPlanType(
        Domain.Entities.PlanType planType)
    {
        if (planType == null)
            return null;

        GetPlanTypeResponse getPlanTypeResponse = new GetPlanTypeResponse();
        getPlanTypeResponse.Id = planType.Id;
        getPlanTypeResponse.Descricao = planType.Description;
        getPlanTypeResponse.DataCriacao = planType.DateCreated;
        getPlanTypeResponse.DataAlteracao = planType.DateUpdated;

        return getPlanTypeResponse;
    }
}