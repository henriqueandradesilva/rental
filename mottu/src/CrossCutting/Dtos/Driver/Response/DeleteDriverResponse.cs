using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Driver.Response;

public class DeleteDriverResponse : BaseDeleteResponse
{
    public DeleteDriverResponse()
    {

    }

    public DeleteDriverResponse(
        Domain.Entities.Driver driver)
    {
        Id = driver.Id;
    }
}