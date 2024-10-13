using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.OrderStatus.Response;

public class GetOrderStatusResponse : BaseResponse
{
    public string Descricao { get; set; }

    public GetOrderStatusResponse()
    {

    }

    public GetOrderStatusResponse GetOrderStatus(
        Domain.Entities.OrderStatus orderStatus)
    {
        if (orderStatus == null)
            return null;

        GetOrderStatusResponse getOrderStatusResponse = new GetOrderStatusResponse();
        getOrderStatusResponse.Id = orderStatus.Id;
        getOrderStatusResponse.Descricao = orderStatus.Description;
        getOrderStatusResponse.DataCriacao = orderStatus.DateCreated;
        getOrderStatusResponse.DataAlteracao = orderStatus.DateUpdated;

        return getOrderStatusResponse;
    }
}