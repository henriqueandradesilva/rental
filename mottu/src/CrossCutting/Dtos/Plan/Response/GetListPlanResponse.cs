using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.PlanType.Response;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.Plan.Response;

public class GetListPlanResponse : BaseResponse
{
    public long TipoPlanoId { get; set; }

    public string Descricao { get; set; }

    public double TaxaDiaria { get; set; }

    public double TaxaAdicional { get; set; }

    public double TaxaFixa { get; set; }

    public int DuracaoEmDias { get; set; }

    public bool Ativo { get; set; }

    public GetPlanTypeResponse TipoPlano { get; set; }

    public GetListPlanResponse()
    {

    }

    public List<GetListPlanResponse> GetListPlan(
        List<Domain.Entities.Plan> listPlan)
    {
        if (listPlan == null)
            return null;

        return listPlan
        .Select(e => new GetListPlanResponse()
        {
            Id = e.Id,
            TipoPlanoId = e.PlanTypeId,
            Descricao = e.Description,
            TaxaDiaria = e.DailyRate,
            TaxaAdicional = e.AdditionalRate,
            TaxaFixa = e.DailyLateFee,
            DuracaoEmDias = e.DurationInDays,
            Ativo = e.IsActive,
            TipoPlano = new GetPlanTypeResponse().GetPlanType(e.PlanType),
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}