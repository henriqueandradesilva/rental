using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.OrderAccepted.Response;

public class PutOrderAcceptedResponse : BaseResponse
{
    public PutOrderAcceptedResponse()
    {

    }

    public PutOrderAcceptedResponse(
        Domain.Entities.OrderAccepted orderAccepted)
    {
        Id = orderAccepted.Id;
        DataCriacao = orderAccepted.DateCreated;
        DataAlteracao = orderAccepted.DateUpdated;
    }
}