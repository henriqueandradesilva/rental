using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.PlanType.Response;

namespace CrossCutting.Dtos.Plan.Response;

public class GetPlanResponse : BaseResponse
{
    public long TipoPlanoId { get; set; }

    public string Descricao { get; set; }

    public double TaxaDiaria { get; set; }

    public double TaxaAdicional { get; set; }

    public double TaxaFixa { get; set; }

    public int DuracaoEmDias { get; set; }

    public bool Ativo { get; set; }

    public GetPlanTypeResponse TipoPlano { get; set; }

    public GetPlanResponse()
    {

    }

    public GetPlanResponse GetPlan(
        Domain.Entities.Plan plan)
    {
        if (plan == null)
            return null;

        GetPlanResponse getPlanResponse = new GetPlanResponse();
        getPlanResponse.Id = plan.Id;
        getPlanResponse.TipoPlanoId = plan.PlanTypeId;
        getPlanResponse.Descricao = plan.Description;
        getPlanResponse.TaxaDiaria = plan.DailyRate;
        getPlanResponse.TaxaAdicional = plan.AdditionalRate;
        getPlanResponse.TaxaFixa = plan.DailyLateFee;
        getPlanResponse.DuracaoEmDias = plan.DurationInDays;
        getPlanResponse.Ativo = plan.IsActive;
        getPlanResponse.TipoPlano = new GetPlanTypeResponse().GetPlanType(plan.PlanType);
        getPlanResponse.DataCriacao = plan.DateCreated;
        getPlanResponse.DataAlteracao = plan.DateUpdated;

        return getPlanResponse;
    }
}