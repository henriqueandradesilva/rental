using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Plan.Request;

public class PutPlanRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.PlanTypeIdRequired)]
    public long TipoPlanoId { get; set; }

    [Required(ErrorMessage = MessageConst.DescriptionRequired)]
    public string Descricao { get; set; }

    [Required(ErrorMessage = MessageConst.PlanDailyRateRequired)]
    public double TaxaDiaria { get; set; }

    [Required(ErrorMessage = MessageConst.PlanAdditionalRateRequired)]
    public double TaxaAdicional { get; set; }

    [Required(ErrorMessage = MessageConst.PlanDailyLateFeeRequired)]
    public double TaxaFixa { get; set; }

    [Required(ErrorMessage = MessageConst.PlanDurationInDaysRequired)]
    public int DuracaoEmDias { get; set; }

    [Required(ErrorMessage = MessageConst.PlanActiveRequired)]
    public bool Ativo { get; set; }

    public PutPlanRequest()
    {

    }

    public PutPlanRequest(
        long tipoPlanoId,
        string descricao,
        double taxaDiaria,
        double taxaAdicional,
        double taxaFixa,
        int duracaoEmDias,
        bool ativo)
    {
        TipoPlanoId = tipoPlanoId;
        Descricao = descricao;
        TaxaDiaria = taxaDiaria;
        TaxaAdicional = taxaAdicional;
        TaxaFixa = taxaFixa;
        DuracaoEmDias = duracaoEmDias;
        Ativo = ativo;
    }
}