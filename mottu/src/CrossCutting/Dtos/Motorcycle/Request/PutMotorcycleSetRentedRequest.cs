using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Motorcycle.Request;

public class PutMotorcycleSetRentedRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.MotorcycleRentedRequired)]
    public bool Alugado { get; set; }

    public PutMotorcycleSetRentedRequest()
    {

    }

    public PutMotorcycleSetRentedRequest(
        bool alugado)
    {
        Alugado = alugado;
    }
}