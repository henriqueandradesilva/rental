using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Order.Response;

public class PostOrderResponse : BaseResponse
{
    public PostOrderResponse()
    {

    }

    public PostOrderResponse(
        Domain.Entities.Order order)
    {
        Id = order.Id;
        DataCriacao = order.DateCreated;
        DataAlteracao = order.DateUpdated;
    }
}