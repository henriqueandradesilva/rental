using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.UserRole.Response;

public class PutUserRoleResponse : BaseResponse
{
    public PutUserRoleResponse()
    {

    }

    public PutUserRoleResponse(
        Domain.Entities.UserRole userRole)
    {
        Id = userRole.Id;
        DataCriacao = userRole.DateCreated;
        DataAlteracao = userRole.DateUpdated;
    }
}