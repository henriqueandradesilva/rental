using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Motorcycle.Response;

public class DeleteMotorcycleResponse : BaseDeleteResponse
{
    public DeleteMotorcycleResponse()
    {

    }

    public DeleteMotorcycleResponse(
        Domain.Entities.Motorcycle motorcycle)
    {
        Id = motorcycle.Id;
    }
}