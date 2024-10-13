using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Rental.Request;

public class PutRentalSetReturnRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.RentalEndDateRequired)]
    public DateTime Data_Devolucao { get; set; }

    public PutRentalSetReturnRequest()
    {

    }

    public PutRentalSetReturnRequest(
        DateTime dataDevolucao)
    {
        Data_Devolucao = dataDevolucao;
    }
}