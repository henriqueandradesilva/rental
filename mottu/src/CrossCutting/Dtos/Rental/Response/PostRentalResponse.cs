using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Rental.Response;

public class PostRentalResponse : BaseResponse
{
    public PostRentalResponse()
    {

    }

    public PostRentalResponse(
        Domain.Entities.Rental rental)
    {
        Id = rental.Id;
        DataCriacao = rental.DateCreated;
        DataAlteracao = rental.DateUpdated;
    }
}