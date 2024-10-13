using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Driver.Response;

public class PutDriverResponse : BaseResponse
{
    public PutDriverResponse()
    {

    }

    public PutDriverResponse(
        Domain.Entities.Driver driver)
    {
        Id = driver.Id;
        DataCriacao = driver.DateCreated;
        DataAlteracao = driver.DateUpdated;
    }
}