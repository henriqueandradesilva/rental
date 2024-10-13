using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.ModelVehicle.Response;

public class DeleteModelVehicleResponse : BaseDeleteResponse
{
    public DeleteModelVehicleResponse()
    {

    }

    public DeleteModelVehicleResponse(
        Domain.Entities.ModelVehicle modelVehicle)
    {
        Id = modelVehicle.Id;
    }
}