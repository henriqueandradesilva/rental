using CrossCutting.Common.Dtos.Response;
using CrossCutting.Dtos.Driver.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Dtos.DriverNotificated.Response;

public class GetListDriverNotificatedResponse : BaseResponse
{
    public long EntregadorId { get; set; }

    public long NotificacaoId { get; set; }

    public DateTime Data { get; set; }

    public GetDriverResponse Driver { get; set; }

    public GetListDriverNotificatedResponse()
    {

    }

    public List<GetListDriverNotificatedResponse> GetListDriverNotificated(
        List<Domain.Entities.DriverNotificated> listDriverNotificated)
    {
        if (listDriverNotificated == null)
            return null;

        return listDriverNotificated
        .Select(e => new GetListDriverNotificatedResponse()
        {
            Id = e.Id,
            EntregadorId = e.DriverId,
            NotificacaoId = e.NotificationId,
            Data = e.Date,
            Driver = new GetDriverResponse().GetDriver(e.Driver),
            DataCriacao = e.DateCreated,
            DataAlteracao = e.DateUpdated
        })
        .ToList();
    }
}