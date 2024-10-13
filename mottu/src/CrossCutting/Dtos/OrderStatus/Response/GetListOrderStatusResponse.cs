using CrossCutting.Common.Dtos.Response;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.OrderStatus.Response;

public class GetListOrderStatusResponse : BaseResponse
{
    public string Descricao { get; set; }

    public GetListOrderStatusResponse()
    {

    }

    public List<GetListOrderStatusResponse> GetListOrderStatus(
        List<Domain.Entities.OrderStatus> listOrderStatus)
    {
        if (listOrderStatus == null)
            return null;

        return listOrderStatus
        .Select(e => new GetListOrderStatusResponse()
        {
            Id = e.Id,
            Descricao = e.Description,
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}