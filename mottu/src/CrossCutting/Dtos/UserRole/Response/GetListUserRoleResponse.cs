using CrossCutting.Common.Dtos.Response;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.UserRole.Response;

public class GetListUserRoleResponse : BaseResponse
{
    public string Descricao { get; set; }

    public GetListUserRoleResponse()
    {

    }

    public List<GetListUserRoleResponse> GetListUserRole(
        List<Domain.Entities.UserRole> listUserRole)
    {
        if (listUserRole == null)
            return null;

        return listUserRole
        .Select(e => new GetListUserRoleResponse()
        {
            Id = e.Id,
            Descricao = e.Description,
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}