using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Driver.Request;

public class PostDriverSetCnhImageRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.DriverCnhImageRequired)]
    public string Imagem_Cnh { get; set; }

    public PostDriverSetCnhImageRequest()
    {

    }

    public PostDriverSetCnhImageRequest(
        string imagemCnh)
    {
        Imagem_Cnh = imagemCnh;
    }
}