using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.OrderAccepted.Response;

public class PostOrderAcceptedResponse : BaseResponse
{
    public PostOrderAcceptedResponse()
    {

    }

    public PostOrderAcceptedResponse(
        Domain.Entities.OrderAccepted orderAccepted)
    {
        Id = orderAccepted.Id;
        DataCriacao = orderAccepted.DateCreated;
        DataAlteracao = orderAccepted.DateUpdated;
    }
}