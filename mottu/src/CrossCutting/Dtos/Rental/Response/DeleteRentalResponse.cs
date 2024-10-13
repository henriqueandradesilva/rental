using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Rental.Response;

public class DeleteRentalResponse : BaseDeleteResponse
{
    public DeleteRentalResponse()
    {

    }

    public DeleteRentalResponse(
        Domain.Entities.Rental rental)
    {
        Id = rental.Id;
    }
}