using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Driver.Request;

public class PutDriverRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.DriverIdentifierRequired)]
    public string Identificador { get; set; }

    [Required(ErrorMessage = MessageConst.DriverNameRequired)]
    public string Nome { get; set; }

    [Required(ErrorMessage = MessageConst.DriverCnpjRequired)]
    public string Cnpj { get; set; }

    [Required(ErrorMessage = MessageConst.DriverDateOfBirthRequired)]
    public DateTime Data_Nascimento { get; set; }

    [Required(ErrorMessage = MessageConst.DriverCnhRequired)]
    public string Numero_Cnh { get; set; }

    [Required(ErrorMessage = MessageConst.DriverCnhTypeRequired)]
    public string Tipo_Cnh { get; set; }

    public string Imagem_Cnh { get; set; }

    public PutDriverRequest()
    {

    }

    public PutDriverRequest(
        string identificador,
        string nome,
        string cnpj,
        DateTime? dataNascimento,
        string numeroCnh,
        string tipoCnh,
        string imagemCnh)
    {
        Identificador = identificador;
        Nome = nome;
        Cnpj = cnpj;

        if (dataNascimento != null)
            Data_Nascimento = dataNascimento.Value;

        Numero_Cnh = numeroCnh;
        Tipo_Cnh = tipoCnh;
        Imagem_Cnh = imagemCnh;
    }
}