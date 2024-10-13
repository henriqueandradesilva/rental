using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Motorcycle.Response;

public class PostMotorcycleResponse : BaseResponse
{
    public PostMotorcycleResponse()
    {

    }

    public PostMotorcycleResponse(
        Domain.Entities.Motorcycle motorcycle)
    {
        Id = motorcycle.Id;
        DataCriacao = motorcycle.DateCreated;
        DataAlteracao = motorcycle.DateUpdated;
    }
}