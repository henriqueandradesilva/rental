using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.Driver.Response;
using CrossCutting.Dtos.Order.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.OrderDelivered.Response;

public class GetListOrderDeliveredResponse : BaseResponse
{
    public long EntregadorId { get; set; }

    public long PedidoId { get; set; }

    public DateTime Data { get; set; }

    public GetDriverResponse Entregador { get; set; }

    public GetOrderResponse Pedido { get; set; }

    public GetListOrderDeliveredResponse()
    {

    }

    public List<GetListOrderDeliveredResponse> GetListOrderDelivered(
        List<Domain.Entities.OrderDelivered> listOrderDelivered)
    {
        if (listOrderDelivered == null)
            return null;

        return listOrderDelivered
        .Select(e => new GetListOrderDeliveredResponse()
        {
            Id = e.Id,
            EntregadorId = e.DriverId,
            PedidoId = e.OrderId,
            Data = e.Date,
            Entregador = new GetDriverResponse().GetDriver(e.Driver),
            Pedido = new GetOrderResponse().GetOrder(e.Order),
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}