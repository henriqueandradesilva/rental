using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.OrderStatus.Response;
using System;

namespace CrossCutting.Dtos.Order.Response;

public class GetOrderResponse : BaseResponse
{
    public long StatusId { get; set; }

    public string Descricao { get; set; }

    public double Valor { get; set; }

    public DateTime Data { get; set; }

    public GetOrderStatusResponse Status { get; set; }

    public GetOrderResponse()
    {

    }

    public GetOrderResponse GetOrder(
        Domain.Entities.Order order)
    {
        if (order == null)
            return null;

        GetOrderResponse getOrderResponse = new GetOrderResponse();
        getOrderResponse.Id = order.Id;
        getOrderResponse.StatusId = order.StatusId;
        getOrderResponse.Descricao = order.Description;
        getOrderResponse.Valor = order.Value;
        getOrderResponse.Data = order.Date;
        getOrderResponse.Status = new GetOrderStatusResponse().GetOrderStatus(order.Status);
        getOrderResponse.DataCriacao = order.DateCreated;
        getOrderResponse.DataAlteracao = order.DateUpdated;

        return getOrderResponse;
    }
}