using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Driver.Response;

public class PostDriverResponse : BaseResponse
{
    public PostDriverResponse()
    {

    }

    public PostDriverResponse(
        Domain.Entities.Driver driver)
    {
        Id = driver.Id;
        DataCriacao = driver.DateCreated;
        DataAlteracao = driver.DateUpdated;
    }
}