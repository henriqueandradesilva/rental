using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.ModelVehicle.Request;

public class PostModelVehicleRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.DescriptionRequired)]
    public string Descricao { get; set; }

    public PostModelVehicleRequest()
    {

    }

    public PostModelVehicleRequest(
        string descricao)
    {
        Descricao = descricao;
    }
}