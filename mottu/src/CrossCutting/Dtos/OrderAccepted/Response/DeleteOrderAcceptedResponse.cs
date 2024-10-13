using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.OrderAccepted.Response;

public class DeleteOrderAcceptedResponse : BaseDeleteResponse
{
    public DeleteOrderAcceptedResponse()
    {

    }

    public DeleteOrderAcceptedResponse(
        Domain.Entities.OrderAccepted orderAccepted)
    {
        Id = orderAccepted.Id;
    }
}