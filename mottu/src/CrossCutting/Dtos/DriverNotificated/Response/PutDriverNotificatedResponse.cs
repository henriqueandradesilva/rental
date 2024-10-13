using CrossCutting.Common.Dtos.Response;

namespace CrossCutting.Dtos.DriverNotificated.Response;

public class PutDriverNotificatedResponse : BaseResponse
{
    public PutDriverNotificatedResponse()
    {

    }

    public PutDriverNotificatedResponse(
        Domain.Entities.DriverNotificated driverNotificated)
    {
        Id = driverNotificated.Id;
        DataCriacao = driverNotificated.DateCreated;
        DataAlteracao = driverNotificated.DateUpdated;
    }
}