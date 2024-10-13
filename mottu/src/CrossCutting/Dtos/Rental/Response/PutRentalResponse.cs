using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Rental.Response;

public class PutRentalResponse : BaseResponse
{
    public PutRentalResponse()
    {

    }

    public PutRentalResponse(
        Domain.Entities.Rental rental)
    {
        Id = rental.Id;
        DataCriacao = rental.DateCreated;
        DataAlteracao = rental.DateUpdated;
    }
}