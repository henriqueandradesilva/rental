using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.UserRole.Response;

public class DeleteUserRoleResponse : BaseDeleteResponse
{
    public DeleteUserRoleResponse()
    {

    }

    public DeleteUserRoleResponse(
        Domain.Entities.UserRole userRole)
    {
        Id = userRole.Id;
    }
}