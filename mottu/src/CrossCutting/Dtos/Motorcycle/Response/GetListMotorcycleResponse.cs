using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.ModelVehicle.Response;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.Motorcycle.Response;

public class GetListMotorcycleResponse : BaseResponse
{
    public string Identificador { get; set; }

    public int Ano { get; set; }

    public string Placa { get; set; }

    public string Modelo { get; set; }

    public bool Alugada { get; set; }

    public GetListMotorcycleResponse()
    {

    }

    public List<GetListMotorcycleResponse> GetListMotorcycle(
        List<Domain.Entities.Motorcycle> listMotorcycle)
    {
        if (listMotorcycle == null)
            return null;

        return listMotorcycle
        .Select(e => new GetListMotorcycleResponse()
        {
            Id = e.Id,
            Identificador = e.Identifier,
            Ano = e.Year,
            Modelo = new GetModelVehicleResponse().GetModelVehicle(e.ModelVehicle).Descricao,
            Placa = e.Plate,
            Alugada = e.IsRented,
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}