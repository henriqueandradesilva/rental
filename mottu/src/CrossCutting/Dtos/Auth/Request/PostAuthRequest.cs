using CrossCutting.Common.Dtos.Request;
using CrossCutting.Const;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.Auth.Request;

public class PostAuthRequest : BaseRequest
{
    [Required(ErrorMessage = SystemConst.AuthEmailRequired)]
    [EmailAddress(ErrorMessage = SystemConst.AuthEmailInvalid)]
    public string Email { get; set; }

    [Required(ErrorMessage = SystemConst.AuthPasswordRequired)]
    public string Senha { get; set; }

    public PostAuthRequest()
    {

    }

    public PostAuthRequest(
        string email,
        string senha)
    {
        Email = email;
        Senha = senha;
    }
}