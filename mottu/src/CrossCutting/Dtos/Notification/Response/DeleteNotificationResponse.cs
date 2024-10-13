using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.Notification.Response;

public class DeleteNotificationResponse : BaseDeleteResponse
{
    public DeleteNotificationResponse()
    {

    }

    public DeleteNotificationResponse(
        Domain.Entities.Notification notification)
    {
        Id = notification.Id;
    }
}