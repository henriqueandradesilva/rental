using Application.UseCases.V1.DriverNotificated.GetListSearchDriverNotificated.Interfaces;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Application.UseCases.V1.DriverNotificated.GetListSearchDriverNotificated;

public class GetListSearchDriverNotificatedUseCase : IGetListSearchDriverNotificatedUseCase
{
    private IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.DriverNotificated>> _outputPort;
    private readonly IDriverNotificatedRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchDriverNotificatedUseCase(
        IDriverNotificatedRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        var genericPaginationResponse = new GenericPaginationResponse<Domain.Entities.DriverNotificated>();

        var normalizedText = genericSearchPaginationRequest.Texto?.NormalizeString();

        var query =
            _repository?.Where(x => string.IsNullOrEmpty(normalizedText) ||
                                     x.Driver.Identifier.ToUpper().Trim().Contains(normalizedText) ||
                                     x.Driver.Name.ToUpper().Trim().Contains(normalizedText) ||
                                     x.Driver.Cnpj.ToUpper().Trim().Contains(normalizedText) ||
                                     x.Driver.Cnh.ToUpper().Trim().Contains(normalizedText) ||
                                     x.Notification.Order.Description.ToUpper().Trim().Contains(normalizedText)
                                    );

        if (genericSearchPaginationRequest.Id.HasValue)
            query = _repository?.Where(x => x.Id == genericSearchPaginationRequest.Id.Value);

        if (genericSearchPaginationRequest.ListaRelacionamento != null &&
            genericSearchPaginationRequest.ListaRelacionamento.Any())
        {
            foreach (var relational in genericSearchPaginationRequest.ListaRelacionamento)
            {
                if (relational.Item1.ToUpper() == SystemConst.FieldDriverId.ToUpper())
                    query = query.Where(c => c.DriverId == relational.Item2);

                if (relational.Item1.ToUpper() == SystemConst.FieldNotificationId.ToUpper())
                    query = query.Where(c => c.NotificationId == relational.Item2);
            }
        }

        query =
            query.Where(x => (!genericSearchPaginationRequest.DataInicio.HasValue || x.Date >= genericSearchPaginationRequest.DataInicio.Value) &&
                             (!genericSearchPaginationRequest.DataFim.HasValue || x.Date <= genericSearchPaginationRequest.DataFim.Value));

        if (!string.IsNullOrEmpty(genericSearchPaginationRequest.CampoOrdenacao))
            query = query?.OrderBy($"{genericSearchPaginationRequest.CampoOrdenacao} {genericSearchPaginationRequest.DirecaoOrdenacao}");

        query = query?.Include(c => c.Driver)
                     ?.Select(x => x);

        genericPaginationResponse.LoadPagination(query, genericSearchPaginationRequest);

        if (genericPaginationResponse == null || genericPaginationResponse.Total == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.DriverNotificatedNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(genericPaginationResponse);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.DriverNotificated>> outputPort)
        => _outputPort = outputPort;
}