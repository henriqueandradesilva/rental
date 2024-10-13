using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.Driver.Response;
using CrossCutting.Dtos.UserRole.Response;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.User.Response;

public class GetListUserResponse : BaseResponse
{
    public long TipoPerfilId { get; set; }

    public long? EntregadorId { get; set; }

    public string Nome { get; set; }

    public string Email { get; set; }

    public bool Ativo { get; set; }

    public GetUserRoleResponse TipoPerfil { get; set; }

    public GetDriverResponse Entregador { get; set; }

    public GetListUserResponse()
    {

    }

    public List<GetListUserResponse> GetListUser(
        List<Domain.Entities.User> listUser)
    {
        if (listUser == null)
            return null;

        return listUser
        .Select(e => new GetListUserResponse()
        {
            Id = e.Id,
            TipoPerfilId = e.UserRoleId,
            EntregadorId = e.Driver != null ? e.Driver.Id : 0,
            Nome = e.Name,
            Email = e.Email,
            Ativo = e.IsActive,
            TipoPerfil = new GetUserRoleResponse().GetUserRole(e.UserRole),
            Entregador = e.Driver != null ? new GetDriverResponse().GetDriver(e.Driver) : null,
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}