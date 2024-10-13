using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.User.Response;

public class DeleteUserResponse : BaseDeleteResponse
{
    public DeleteUserResponse()
    {

    }

    public DeleteUserResponse(
        Domain.Entities.User user)
    {
        Id = user.Id;
    }
}