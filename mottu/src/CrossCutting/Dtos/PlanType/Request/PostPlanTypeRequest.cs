using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.PlanType.Request;

public class PostPlanTypeRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.DescriptionRequired)]
    public string Descricao { get; set; }

    public PostPlanTypeRequest()
    {

    }

    public PostPlanTypeRequest(
        string descricao)
    {
        Descricao = descricao;
    }
}