using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.Order.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.Notification.Response;

public class GetListNotificationResponse : BaseResponse
{
    public long PedidoId { get; set; }

    public DateTime Data { get; set; }

    public GetOrderResponse Pedido { get; set; }

    public GetListNotificationResponse()
    {

    }

    public List<GetListNotificationResponse> GetListNotification(
        List<Domain.Entities.Notification> listNotification)
    {
        if (listNotification == null)
            return null;

        return listNotification
        .Select(e => new GetListNotificationResponse()
        {
            Id = e.Id,
            PedidoId = e.OrderId,
            Data = e.Date,
            Pedido = new GetOrderResponse().GetOrder(e.Order),
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated,
        })
        .ToList();
    }
}