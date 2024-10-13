using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.DriverNotificated.Response;

public class DeleteDriverNotificatedResponse : BaseDeleteResponse
{
    public DeleteDriverNotificatedResponse()
    {

    }

    public DeleteDriverNotificatedResponse(
        Domain.Entities.DriverNotificated driverNotificated)
    {
        Id = driverNotificated.Id;
    }
}