using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.OrderStatus.Response;

public class DeleteOrderStatusResponse : BaseDeleteResponse
{
    public DeleteOrderStatusResponse()
    {

    }

    public DeleteOrderStatusResponse(
        Domain.Entities.OrderStatus orderStatus)
    {
        Id = orderStatus.Id;
    }
}