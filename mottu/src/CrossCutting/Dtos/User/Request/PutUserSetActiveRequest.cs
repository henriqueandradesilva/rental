using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.User.Request;

public class PutUserSetActiveRequest : BaseRequest
{

    [Required(ErrorMessage = MessageConst.UserActiveRequired)]
    public bool Ativo { get; set; }

    public PutUserSetActiveRequest()
    {

    }

    public PutUserSetActiveRequest(
        bool ativo)
    {
        Ativo = ativo;
    }
}