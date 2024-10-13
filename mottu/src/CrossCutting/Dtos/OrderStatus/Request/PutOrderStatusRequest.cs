using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.OrderStatus.Request;

public class PutOrderStatusRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.DescriptionRequired)]
    public string Descricao { get; set; }

    public PutOrderStatusRequest()
    {

    }

    public PutOrderStatusRequest(
        string descricao)
    {
        Descricao = descricao;
    }
}