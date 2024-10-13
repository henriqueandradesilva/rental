using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Motorcycle.Response;

public class PutMotorcycleResponse : BaseResponse
{
    public PutMotorcycleResponse()
    {

    }

    public PutMotorcycleResponse(
        Domain.Entities.Motorcycle motorcycle)
    {
        Id = motorcycle.Id;
        DataCriacao = motorcycle.DateCreated;
        DataAlteracao = motorcycle.DateUpdated;
    }
}