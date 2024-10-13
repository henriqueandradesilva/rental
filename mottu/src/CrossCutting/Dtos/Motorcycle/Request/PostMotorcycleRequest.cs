using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Motorcycle.Request;

public class PostMotorcycleRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.MotorcycleIdentifierRequired)]
    public string Identificador { get; set; }

    [Required(ErrorMessage = MessageConst.MotorcycleYearRequired)]
    public int Ano { get; set; }

    [Required(ErrorMessage = MessageConst.MotorcycleModelVehicleIdRequired)]
    public string Modelo { get; set; }

    [Required(ErrorMessage = MessageConst.MotorcyclePlateRequired)]
    public string Placa { get; set; }

    public PostMotorcycleRequest()
    {

    }

    public PostMotorcycleRequest(
        string identificador,
        int ano,
        string modelo,
        string placa)
    {
        Identificador = identificador;
        Ano = ano;
        Modelo = modelo;
        Placa = placa;
    }
}