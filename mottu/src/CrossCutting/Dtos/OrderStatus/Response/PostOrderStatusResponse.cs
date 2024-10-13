using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.OrderStatus.Response;

public class PostOrderStatusResponse : BaseResponse
{
    public PostOrderStatusResponse()
    {

    }

    public PostOrderStatusResponse(
        Domain.Entities.OrderStatus orderStatus)
    {
        Id = orderStatus.Id;
        DataCriacao = orderStatus.DateCreated;
        DataAlteracao = orderStatus.DateUpdated;
    }
}