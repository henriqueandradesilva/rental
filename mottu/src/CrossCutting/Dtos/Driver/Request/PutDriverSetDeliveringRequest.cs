using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Driver.Request;

public class PutDriverSetDeliveringRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.DriverDeliveringRequired)]
    public bool Entregando { get; set; }

    public PutDriverSetDeliveringRequest()
    {

    }

    public PutDriverSetDeliveringRequest(
        bool entregando)
    {
        Entregando = entregando;
    }
}