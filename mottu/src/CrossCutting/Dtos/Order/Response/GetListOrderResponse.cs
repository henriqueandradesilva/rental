using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.OrderStatus.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.Order.Response;

public class GetListOrderResponse : BaseResponse
{
    public long StatusId { get; set; }

    public string Descricao { get; set; }

    public double Valor { get; set; }

    public DateTime Data { get; set; }

    public GetOrderStatusResponse Status { get; set; }

    public GetListOrderResponse()
    {

    }

    public List<GetListOrderResponse> GetListOrder(
        List<Domain.Entities.Order> listOrder)
    {
        if (listOrder == null)
            return null;

        return listOrder
        .Select(e => new GetListOrderResponse()
        {
            Id = e.Id,
            StatusId = e.StatusId,
            Valor = e.Value,
            Data = e.Date,
            Status = new GetOrderStatusResponse().GetOrderStatus(e.Status),
            Descricao = e.Description,
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}