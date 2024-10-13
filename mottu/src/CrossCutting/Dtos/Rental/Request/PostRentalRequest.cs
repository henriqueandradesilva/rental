using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Rental.Request;

public class PostRentalRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.RentalDriverIdRequired)]
    public long Entregador_Id { get; set; }

    [Required(ErrorMessage = MessageConst.RentalMotorcycleIdRequired)]
    public long Moto_Id { get; set; }

    [Required(ErrorMessage = MessageConst.RentalPlanIdRequired)]
    public long Plano_Id { get; set; }

    [Required(ErrorMessage = MessageConst.RentalStartDateRequired)]
    public DateTime Data_Inicio { get; set; }

    public PostRentalRequest()
    {

    }

    public PostRentalRequest(
        long entregadorId,
        long motoId,
        long planoId,
        DateTime dataInicio)
    {
        Entregador_Id = entregadorId;
        Moto_Id = motoId;
        Plano_Id = planoId;
        Data_Inicio = dataInicio;
    }
}