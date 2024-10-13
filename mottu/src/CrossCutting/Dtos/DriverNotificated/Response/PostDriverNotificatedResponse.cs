using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.DriverNotificated.Response;

public class PostDriverNotificatedResponse : BaseResponse
{
    public PostDriverNotificatedResponse()
    {

    }

    public PostDriverNotificatedResponse(
        Domain.Entities.DriverNotificated driverNotificated)
    {
        Id = driverNotificated.Id;
        DataCriacao = driverNotificated.DateCreated;
        DataAlteracao = driverNotificated.DateUpdated;
    }
}