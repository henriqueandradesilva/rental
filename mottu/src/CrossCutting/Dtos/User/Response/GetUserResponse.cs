using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.Driver.Response;
using CrossCutting.Dtos.UserRole.Response;

namespace CrossCutting.Dtos.User.Response;

public class GetUserResponse : BaseResponse
{
    public long TipoPerfilId { get; set; }

    public long? EntregadorId { get; set; }

    public string Nome { get; set; }

    public string Email { get; set; }

    public bool Ativo { get; set; }

    public string Senha { get; set; }

    public GetUserRoleResponse TipoPerfil { get; set; }

    public GetDriverResponse Entregador { get; set; }

    public GetUserResponse()
    {

    }

    public GetUserResponse GetUser(
        Domain.Entities.User user)
    {
        if (user == null)
            return null;

        GetUserResponse getUserResponse = new GetUserResponse();
        getUserResponse.Id = user.Id;
        getUserResponse.TipoPerfilId = user.UserRoleId;
        getUserResponse.EntregadorId = user.Driver != null ? user.Driver.Id : 0;
        getUserResponse.Nome = user.Name;
        getUserResponse.Email = user.Email;
        getUserResponse.Ativo = user.IsActive;
        getUserResponse.TipoPerfil = new GetUserRoleResponse().GetUserRole(user.UserRole);
        getUserResponse.Entregador = user.Driver != null ? new GetDriverResponse().GetDriver(user.Driver) : null;
        getUserResponse.DataCriacao = user.DateCreated;
        getUserResponse.DataAlteracao = user.DateUpdated;

        return getUserResponse;
    }
}