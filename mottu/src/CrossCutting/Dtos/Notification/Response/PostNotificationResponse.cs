using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Notification.Response;

public class PostNotificationResponse : BaseResponse
{
    public PostNotificationResponse()
    {

    }

    public PostNotificationResponse(
        Domain.Entities.Notification notification)
    {
        Id = notification.Id;
        DataCriacao = notification.DateCreated;
        DataAlteracao = notification.DateUpdated;
    }
}