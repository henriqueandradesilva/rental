using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.PlanType.Request;

public class PutPlanTypeRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.DescriptionRequired)]
    public string Descricao { get; set; }

    public PutPlanTypeRequest()
    {

    }

    public PutPlanTypeRequest(
        string descricao)
    {
        Descricao = descricao;
    }
}