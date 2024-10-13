using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.ModelVehicle.Response;

public class PostModelVehicleResponse : BaseResponse
{
    public PostModelVehicleResponse()
    {

    }

    public PostModelVehicleResponse(
        Domain.Entities.ModelVehicle modelVehicle)
    {
        Id = modelVehicle.Id;
        DataCriacao = modelVehicle.DateCreated;
        DataAlteracao = modelVehicle.DateUpdated;
    }
}