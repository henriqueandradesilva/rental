using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.UserRole.Response;

public class PostUserRoleResponse : BaseResponse
{
    public PostUserRoleResponse()
    {

    }

    public PostUserRoleResponse(
        Domain.Entities.UserRole userRole)
    {
        Id = userRole.Id;
        DataCriacao = userRole.DateCreated;
        DataAlteracao = userRole.DateUpdated;
    }
}