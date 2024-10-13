using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.OrderStatus.Response;

public class PutOrderStatusResponse : BaseResponse
{
    public PutOrderStatusResponse()
    {

    }

    public PutOrderStatusResponse(
        Domain.Entities.OrderStatus orderStatus)
    {
        Id = orderStatus.Id;
        DataCriacao = orderStatus.DateCreated;
        DataAlteracao = orderStatus.DateUpdated;
    }
}