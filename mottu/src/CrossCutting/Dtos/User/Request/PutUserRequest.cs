using CrossCutting.Common.Dtos.Request;
using Domain.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace CrossCutting.Dtos.User.Request;

public class PutUserRequest : BaseRequest
{
    [Required(ErrorMessage = MessageConst.UserUserRoleIdRequired)]
    public long TipoPerfilId { get; set; }

    [Required(ErrorMessage = MessageConst.UserNameRequired)]
    public string Nome { get; set; }

    [Required(ErrorMessage = MessageConst.UserEmailRequired)]
    [EmailAddress(ErrorMessage = MessageConst.UserEmailInvalid)]
    public string Email { get; set; }

    [Required(ErrorMessage = MessageConst.UserPasswordRequired)]
    public string Senha { get; set; }

    [Required(ErrorMessage = MessageConst.UserActiveRequired)]
    public bool Ativo { get; set; }

    public PutUserRequest()
    {

    }

    public PutUserRequest(
        long tipoPerfilId,
        string nome,
        string email,
        string senha,
        bool ativo)
    {
        TipoPerfilId = tipoPerfilId;
        Nome = nome;
        Email = email;
        Senha = senha;
        Ativo = ativo;
    }
}