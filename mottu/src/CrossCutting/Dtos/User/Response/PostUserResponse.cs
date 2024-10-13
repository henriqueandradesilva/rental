using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.User.Response;

public class PostUserResponse : BaseResponse
{
    public PostUserResponse()
    {

    }

    public PostUserResponse(
        Domain.Entities.User user)
    {
        Id = user.Id;
        DataCriacao = user.DateCreated;
        DataAlteracao = user.DateUpdated;
    }
}