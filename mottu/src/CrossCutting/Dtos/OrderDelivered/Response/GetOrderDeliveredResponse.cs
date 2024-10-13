using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.Driver.Response;
using CrossCutting.Dtos.Order.Response;
using System;

namespace CrossCutting.Dtos.OrderDelivered.Response;

public class GetOrderDeliveredResponse : BaseResponse
{
    public long EntregadorId { get; set; }

    public long PedidoId { get; set; }

    public DateTime Data { get; set; }

    public GetDriverResponse Entregador { get; set; }

    public GetOrderResponse Pedido { get; set; }

    public GetOrderDeliveredResponse()
    {

    }

    public GetOrderDeliveredResponse GetOrderDelivered(
        Domain.Entities.OrderDelivered orderDelivered)
    {
        if (orderDelivered == null)
            return null;

        GetOrderDeliveredResponse getOrderDeliveredResponse = new GetOrderDeliveredResponse();
        getOrderDeliveredResponse.Id = orderDelivered.Id;
        getOrderDeliveredResponse.EntregadorId = orderDelivered.DriverId;
        getOrderDeliveredResponse.PedidoId = orderDelivered.OrderId;
        getOrderDeliveredResponse.Data = orderDelivered.Date;
        getOrderDeliveredResponse.Entregador = new GetDriverResponse().GetDriver(orderDelivered.Driver);
        getOrderDeliveredResponse.Pedido = new GetOrderResponse().GetOrder(orderDelivered.Order);
        getOrderDeliveredResponse.DataCriacao = orderDelivered.DateCreated;
        getOrderDeliveredResponse.DataAlteracao = orderDelivered.DateUpdated;

        return getOrderDeliveredResponse;
    }
}