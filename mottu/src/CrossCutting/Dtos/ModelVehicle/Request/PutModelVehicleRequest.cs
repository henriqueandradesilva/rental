using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.ModelVehicle.Request;

public class PutModelVehicleRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.DescriptionRequired)]
    public string Descricao { get; set; }

    public PutModelVehicleRequest()
    {

    }

    public PutModelVehicleRequest(
        string descricao)
    {
        Descricao = descricao;
    }
}