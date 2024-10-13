using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.OrderDelivered.Request;

public class PostOrderDeliveredRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.OrderDeliveredDriverIdRequired)]
    public long EntregadorId { get; set; }

    [Required(ErrorMessage = MessageConst.OrderDeliveredOrderIdRequired)]
    public long PedidoId { get; set; }

    [Required(ErrorMessage = MessageConst.OrderDeliveredDateRequired)]
    public DateTime Data { get; set; }

    public PostOrderDeliveredRequest()
    {

    }

    public PostOrderDeliveredRequest(
        long entregadorId,
        long pedidoId,
        DateTime data)
    {
        EntregadorId = entregadorId;
        PedidoId = pedidoId;
        Data = data;
    }
}