using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.Order.Response;
using System;

namespace CrossCutting.Dtos.Notification.Response;

public class GetNotificationResponse : BaseResponse
{
    public long PedidoId { get; set; }

    public DateTime Data { get; set; }

    public GetOrderResponse Pedido { get; set; }

    public GetNotificationResponse()
    {

    }

    public GetNotificationResponse GetNotification(
        Domain.Entities.Notification notification)
    {
        if (notification == null)
            return null;

        GetNotificationResponse getNotificationResponse = new GetNotificationResponse();
        getNotificationResponse.Id = notification.Id;
        getNotificationResponse.PedidoId = notification.OrderId;
        getNotificationResponse.Data = notification.Date;
        getNotificationResponse.Pedido = new GetOrderResponse().GetOrder(notification.Order);
        getNotificationResponse.DataCriacao = notification.DateCreated;
        getNotificationResponse.DataAlteracao = notification.DateUpdated;

        return getNotificationResponse;
    }
}