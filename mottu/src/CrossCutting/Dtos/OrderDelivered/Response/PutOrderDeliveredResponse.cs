using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.OrderDelivered.Response;

public class PutOrderDeliveredResponse : BaseResponse
{
    public PutOrderDeliveredResponse()
    {

    }

    public PutOrderDeliveredResponse(
        Domain.Entities.OrderDelivered orderDelivered)
    {
        Id = orderDelivered.Id;
        DataCriacao = orderDelivered.DateCreated;
        DataAlteracao = orderDelivered.DateUpdated;
    }
}