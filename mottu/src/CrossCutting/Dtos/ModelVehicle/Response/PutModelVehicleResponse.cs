using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.ModelVehicle.Response;

public class PutModelVehicleResponse : BaseResponse
{
    public PutModelVehicleResponse()
    {

    }

    public PutModelVehicleResponse(
        Domain.Entities.ModelVehicle modelVehicle)
    {
        Id = modelVehicle.Id;
        DataCriacao = modelVehicle.DateCreated;
        DataAlteracao = modelVehicle.DateUpdated;
    }
}