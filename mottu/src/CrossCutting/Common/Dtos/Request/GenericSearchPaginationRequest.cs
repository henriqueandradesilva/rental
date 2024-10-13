using System;
using System.Collections.Generic;

namespace CrossCutting.Common.Dtos.Request;

public class GenericSearchPaginationRequest
{
    public long? Id { get; set; }

    public List<Tuple<string, long>> ListaRelacionamento { get; set; }

    public List<Tuple<string, long>> ListaEnum { get; set; }

    public List<Tuple<string, DateTime?, DateTime?>> ListaData { get; set; }

    public string Texto { get; set; }

    public bool? Ativo { get; set; }

    public bool? Alugado { get; set; }

    public DateTime? DataInicio { get; set; }

    public DateTime? DataFim { get; set; }

    public int PaginaAtual { get; set; } = 1;

    public int TamanhoPagina { get; set; } = 100;

    public string CampoOrdenacao { get; set; }

    public string DirecaoOrdenacao { get; set; }

    public bool IgnorarPaginacao { get; set; }

    public int Skip { get { return ((PaginaAtual > 0 ? PaginaAtual : 1) - 1) * TamanhoPagina; } }
}