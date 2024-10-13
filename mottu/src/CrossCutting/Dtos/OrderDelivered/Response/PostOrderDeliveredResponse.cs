using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.OrderDelivered.Response;

public class PostOrderDeliveredResponse : BaseResponse
{
    public PostOrderDeliveredResponse()
    {

    }

    public PostOrderDeliveredResponse(
        Domain.Entities.OrderDelivered orderDelivered)
    {
        Id = orderDelivered.Id;
        DataCriacao = orderDelivered.DateCreated;
        DataAlteracao = orderDelivered.DateUpdated;
    }
}