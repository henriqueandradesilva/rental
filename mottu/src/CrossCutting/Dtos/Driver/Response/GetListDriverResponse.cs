using CrossCutting.Common.Dtos.Response;
using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.Driver.Response;

public class GetListDriverResponse : BaseResponse
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

    public GetListDriverResponse()
    {

    }

    public List<GetListDriverResponse> GetListDriver(
        List<Domain.Entities.Driver> listDriver)
    {
        if (listDriver == null)
            return null;

        return listDriver
        .Select(e => new GetListDriverResponse()
        {
            Id = e.Id,
            UsuarioId = e.UserId,
            Identificador = e.Identifier,
            Nome = e.Name,
            Cnpj = e.Cnpj,
            Data_Nascimento = e.DateOfBirth,
            Numero_Cnh = e.Cnh,
            Tipo_Cnh = e.Type,
            Imagem_Cnh = e.CnhImageUrl,
            Entregando = e.Delivering,
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}