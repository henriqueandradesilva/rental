using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Motorcycle.Request;

public class PutMotorcycleSetPlateRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.MotorcyclePlateRequired)]
    public string Placa { get; set; }

    public PutMotorcycleSetPlateRequest()
    {

    }

    public PutMotorcycleSetPlateRequest(
        string placa)
    {
        Placa = placa;
    }
}