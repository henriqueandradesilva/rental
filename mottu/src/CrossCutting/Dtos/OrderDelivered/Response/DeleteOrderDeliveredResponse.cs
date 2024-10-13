using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.OrderDelivered.Response;

public class DeleteOrderDeliveredResponse : BaseDeleteResponse
{
    public DeleteOrderDeliveredResponse()
    {

    }

    public DeleteOrderDeliveredResponse(
        Domain.Entities.OrderDelivered orderDelivered)
    {
        Id = orderDelivered.Id;
    }
}