using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.Driver.Response;
using CrossCutting.Dtos.Order.Response;
using System;

namespace CrossCutting.Dtos.OrderAccepted.Response;

public class GetOrderAcceptedResponse : BaseResponse
{
    public long EntregadorId { get; set; }

    public long PedidoId { get; set; }

    public DateTime Data { get; set; }

    public GetDriverResponse Entregador { get; set; }

    public GetOrderResponse Pedido { get; set; }

    public GetOrderAcceptedResponse()
    {

    }

    public GetOrderAcceptedResponse GetOrderAccepted(
        Domain.Entities.OrderAccepted orderAccepted)
    {
        if (orderAccepted == null)
            return null;

        GetOrderAcceptedResponse getOrderAcceptedResponse = new GetOrderAcceptedResponse();
        getOrderAcceptedResponse.Id = orderAccepted.Id;
        getOrderAcceptedResponse.EntregadorId = orderAccepted.DriverId;
        getOrderAcceptedResponse.PedidoId = orderAccepted.OrderId;
        getOrderAcceptedResponse.Data = orderAccepted.Date;
        getOrderAcceptedResponse.Entregador = new GetDriverResponse().GetDriver(orderAccepted.Driver);
        getOrderAcceptedResponse.Pedido = new GetOrderResponse().GetOrder(orderAccepted.Order);
        getOrderAcceptedResponse.DataCriacao = orderAccepted.DateCreated;
        getOrderAcceptedResponse.DataAlteracao = orderAccepted.DateUpdated;

        return getOrderAcceptedResponse;
    }
}