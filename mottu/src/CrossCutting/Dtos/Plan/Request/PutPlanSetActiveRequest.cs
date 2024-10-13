using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Plan.Request;

public class PutPlanSetActiveRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.PlanActiveRequired)]
    public bool Ativo { get; set; }

    public PutPlanSetActiveRequest()
    {

    }

    public PutPlanSetActiveRequest(
        bool ativo)
    {
        Ativo = ativo;
    }
}