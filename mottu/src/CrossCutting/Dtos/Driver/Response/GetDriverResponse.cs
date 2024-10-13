using CrossCutting.Common.Dtos.Response;
using Domain.Common.Enums;
using System;

namespace CrossCutting.Dtos.Driver.Response;

public class GetDriverResponse : BaseResponse
{
    public long? UsuarioId { get; set; }

    public string Identificador { get; set; }

    public string Nome { get; set; }

    public string Cnpj { get; set; }

    public DateOnly Data_Nascimento { get; set; }

    public string Numero_Cnh { get; set; }

    public CnhTypeEnum Tipo_Cnh { get; set; }

    public string Imagem_Cnh { get; set; }

    public bool Entregando { get; set; }

    public GetDriverResponse()
    {

    }

    public GetDriverResponse GetDriver(
        Domain.Entities.Driver driver)
    {
        if (driver == null)
            return null;

        GetDriverResponse getDriverResponse = new GetDriverResponse();
        getDriverResponse.Id = driver.Id;
        getDriverResponse.UsuarioId = driver.UserId;
        getDriverResponse.Identificador = driver.Identifier;
        getDriverResponse.Nome = driver.Name;
        getDriverResponse.Cnpj = driver.Cnpj;
        getDriverResponse.Data_Nascimento = driver.DateOfBirth;
        getDriverResponse.Numero_Cnh = driver.Cnh;
        getDriverResponse.Tipo_Cnh = driver.Type;
        getDriverResponse.Imagem_Cnh = driver.CnhImageUrl;
        getDriverResponse.Entregando = driver.Delivering;
        getDriverResponse.DataCriacao = driver.DateCreated;
        getDriverResponse.DataAlteracao = driver.DateUpdated;

        return getDriverResponse;
    }
}