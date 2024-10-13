using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.UserRole.Response;

public class GetUserRoleResponse : BaseResponse
{
    public string Descricao { get; set; }

    public GetUserRoleResponse()
    {

    }

    public GetUserRoleResponse GetUserRole(
        Domain.Entities.UserRole userRole)
    {
        if (userRole == null)
            return null;

        GetUserRoleResponse getUserRoleResponse = new GetUserRoleResponse();
        getUserRoleResponse.Id = userRole.Id;
        getUserRoleResponse.Descricao = userRole.Description;
        getUserRoleResponse.DataCriacao = userRole.DateCreated;
        getUserRoleResponse.DataAlteracao = userRole.DateUpdated;

        return getUserRoleResponse;
    }
}