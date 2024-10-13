using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.UserRole.Request;

public class PostUserRoleRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.DescriptionRequired)]
    public string Descricao { get; set; }

    public PostUserRoleRequest()
    {

    }

    public PostUserRoleRequest(
        string descricao)
    {
        Descricao = descricao;
    }
}