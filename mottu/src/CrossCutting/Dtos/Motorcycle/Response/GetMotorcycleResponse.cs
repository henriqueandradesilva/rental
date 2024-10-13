using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.ModelVehicle.Response;

namespace CrossCutting.Dtos.Motorcycle.Response;

public class GetMotorcycleResponse : BaseResponse
{
    public string Identificador { get; set; }

    public int Ano { get; set; }

    public string Modelo { get; set; }

    public string Placa { get; set; }

    public bool Alugada { get; set; }

    public GetMotorcycleResponse()
    {

    }

    public GetMotorcycleResponse GetMotorcycle(
        Domain.Entities.Motorcycle motorcycle)
    {
        if (motorcycle == null)
            return null;

        GetMotorcycleResponse getMotorcycleResponse = new GetMotorcycleResponse();
        getMotorcycleResponse.Id = motorcycle.Id;
        getMotorcycleResponse.Identificador = motorcycle.Identifier;
        getMotorcycleResponse.Ano = motorcycle.Year;
        getMotorcycleResponse.Modelo = motorcycle.ModelVehicle != null ? new GetModelVehicleResponse().GetModelVehicle(motorcycle.ModelVehicle).Descricao : null;
        getMotorcycleResponse.Placa = motorcycle.Plate;
        getMotorcycleResponse.Alugada = motorcycle.IsRented;
        getMotorcycleResponse.DataCriacao = motorcycle.DateCreated;
        getMotorcycleResponse.DataAlteracao = motorcycle.DateUpdated;

        return getMotorcycleResponse;
    }
}