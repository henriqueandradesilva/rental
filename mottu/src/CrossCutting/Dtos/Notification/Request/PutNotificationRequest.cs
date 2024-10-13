using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Notification.Request;

public class PutNotificationRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.NotificationOrderIdRequired)]
    public long PedidoId { get; set; }

    [Required(ErrorMessage = MessageConst.NotificationDateRequired)]
    public DateTime Data { get; set; }

    public PutNotificationRequest()
    {

    }

    public PutNotificationRequest(
        long pedidoId,
        DateTime data)
    {
        PedidoId = pedidoId;
        Data = data;
    }
}