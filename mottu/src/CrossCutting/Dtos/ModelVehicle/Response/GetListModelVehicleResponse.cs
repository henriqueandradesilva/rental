using CrossCutting.Common.Dtos.Response;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.ModelVehicle.Response;

public class GetListModelVehicleResponse : BaseResponse
{
    public string Descricao { get; set; }

    public GetListModelVehicleResponse()
    {

    }

    public List<GetListModelVehicleResponse> GetListModelVehicle(
        List<Domain.Entities.ModelVehicle> listModelVehicle)
    {
        if (listModelVehicle == null)
            return null;

        return listModelVehicle
        .Select(e => new GetListModelVehicleResponse()
        {
            Id = e.Id,
            Descricao = e.Description,
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}