using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.DriverNotificated.Request;

public class PutDriverNotificatedRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.DriverNotificatedDriverIdRequired)]
    public long EntregadorId { get; set; }

    [Required(ErrorMessage = MessageConst.DriverNotificatedNotificationIdRequired)]
    public long NotificacaoId { get; set; }

    [Required(ErrorMessage = MessageConst.DriverNotificatedDateRequired)]
    public DateTime Data { get; set; }

    public PutDriverNotificatedRequest()
    {

    }

    public PutDriverNotificatedRequest(
        long entregadorId,
        long notificacaoId,
        DateTime data)
    {
        EntregadorId = entregadorId;
        NotificacaoId = notificacaoId;
        Data = data;
    }
}