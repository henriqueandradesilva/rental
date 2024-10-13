using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.UserRole.Request;

public class PutUserRoleRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.DescriptionRequired)]
    public string Descricao { get; set; }

    public PutUserRoleRequest()
    {

    }

    public PutUserRoleRequest(
        string descricao)
    {
        Descricao = descricao;
    }
}