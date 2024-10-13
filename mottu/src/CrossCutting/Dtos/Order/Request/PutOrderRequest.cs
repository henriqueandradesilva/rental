using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Order.Request;

public class PutOrderRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.DescriptionRequired)]
    public string Descricao { get; set; }

    [Required(ErrorMessage = MessageConst.OrderValueRequired)]
    public double Valor { get; set; }

    [Required(ErrorMessage = MessageConst.OrderDateRequired)]
    public DateTime Data { get; set; }


    public PutOrderRequest()
    {

    }

    public PutOrderRequest(
        string descricao,
        double valor,
        DateTime data)
    {
        Descricao = descricao;
        Valor = valor;
        Data = data;
    }
}