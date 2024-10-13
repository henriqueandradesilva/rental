using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.OrderStatus.Request;

public class PostOrderStatusRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.DescriptionRequired)]
    public string Descricao { get; set; }

    public PostOrderStatusRequest()
    {

    }

    public PostOrderStatusRequest(
        string descricao)
    {
        Descricao = descricao;
    }
}