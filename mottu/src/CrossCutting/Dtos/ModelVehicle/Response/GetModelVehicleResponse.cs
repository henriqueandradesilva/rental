using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.ModelVehicle.Response;

public class GetModelVehicleResponse : BaseResponse
{
    public string Descricao { get; set; }

    public GetModelVehicleResponse()
    {

    }

    public GetModelVehicleResponse GetModelVehicle(
        Domain.Entities.ModelVehicle modelVehicle)
    {
        if (modelVehicle == null)
            return null;

        GetModelVehicleResponse getModelVehicleResponse = new GetModelVehicleResponse();
        getModelVehicleResponse.Id = modelVehicle.Id;
        getModelVehicleResponse.Descricao = modelVehicle.Description;
        getModelVehicleResponse.DataCriacao = modelVehicle.DateCreated;
        getModelVehicleResponse.DataAlteracao = modelVehicle.DateUpdated;

        return getModelVehicleResponse;
    }
}