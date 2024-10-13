using System;

namespace CrossCutting.Common.Dtos.Response;

public class BaseResponse
{
    public long Id { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime? DataAlteracao { get; set; }

    public BaseResponse()
    {

    }
}