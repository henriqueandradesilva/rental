using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.Driver.Response;
using System;

namespace CrossCutting.Dtos.DriverNotificated.Response;

public class GetDriverNotificatedResponse : BaseResponse
{
    public long EntregadorId { get; set; }

    public long NotificacaoId { get; set; }

    public DateTime Data { get; set; }

    public GetDriverResponse Driver { get; set; }

    public GetDriverNotificatedResponse()
    {

    }

    public GetDriverNotificatedResponse GetDriverNotificated(
        Domain.Entities.DriverNotificated driverNotificated)
    {
        if (driverNotificated == null)
            return null;

        GetDriverNotificatedResponse getDriverNotificatedResponse = new GetDriverNotificatedResponse();
        getDriverNotificatedResponse.Id = driverNotificated.Id;
        getDriverNotificatedResponse.EntregadorId = driverNotificated.DriverId;
        getDriverNotificatedResponse.NotificacaoId = driverNotificated.NotificationId;
        getDriverNotificatedResponse.Data = driverNotificated.Date;
        getDriverNotificatedResponse.Driver = new GetDriverResponse().GetDriver(driverNotificated.Driver);
        getDriverNotificatedResponse.DataCriacao = driverNotificated.DateCreated;
        getDriverNotificatedResponse.DataAlteracao = driverNotificated.DateUpdated;

        return getDriverNotificatedResponse;
    }
}