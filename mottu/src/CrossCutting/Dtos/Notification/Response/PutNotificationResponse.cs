using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Notification.Response;

public class PutNotificationResponse : BaseResponse
{
    public PutNotificationResponse()
    {

    }

    public PutNotificationResponse(
        Domain.Entities.Notification notification)
    {

        Id = notification.Id;
        DataCriacao = notification.DateCreated;
        DataAlteracao = notification.DateUpdated;
    }
}