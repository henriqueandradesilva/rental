using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Order.Response;

public class DeleteOrderResponse : BaseDeleteResponse
{
    public DeleteOrderResponse()
    {

    }

    public DeleteOrderResponse(
        Domain.Entities.Order order)
    {
        Id = order.Id;
    }
}