using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.OrderAccepted.Request;

public class PostOrderAcceptedRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.OrderAcceptedDriverIdRequired)]
    public long EntregadorId { get; set; }

    [Required(ErrorMessage = MessageConst.OrderAcceptedOrderIdRequired)]
    public long PedidoId { get; set; }

    [Required(ErrorMessage = MessageConst.OrderAcceptedDateRequired)]
    public DateTime Data { get; set; }

    public PostOrderAcceptedRequest()
    {

    }

    public PostOrderAcceptedRequest(
        long entregadorId,
        long pedidoId,
        DateTime data)
    {
        EntregadorId = entregadorId;
        PedidoId = pedidoId;
        Data = data;
    }
}