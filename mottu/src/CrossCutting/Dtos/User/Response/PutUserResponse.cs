using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.User.Response;

public class PutUserResponse : BaseResponse
{
    public PutUserResponse()
    {

    }

    public PutUserResponse(
        Domain.Entities.User user)
    {
        Id = user.Id;
        DataCriacao = user.DateCreated;
        DataAlteracao = user.DateUpdated;
    }
}