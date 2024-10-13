using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Order.Response;

public class PutOrderResponse : BaseResponse
{
    public PutOrderResponse()
    {

    }

    public PutOrderResponse(
        Domain.Entities.Order order)
    {

        Id = order.Id;
        DataCriacao = order.DateCreated;
        DataAlteracao = order.DateUpdated;
    }
}