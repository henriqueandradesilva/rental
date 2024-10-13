using Application.UseCases.V1.Notification.GetListSearchNotification.Interfaces;
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

namespace Application.UseCases.V1.Notification.GetListSearchNotification;

public class GetListSearchNotificationUseCase : IGetListSearchNotificationUseCase
{
    private IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Notification>> _outputPort;
    private readonly INotificationRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchNotificationUseCase(
        INotificationRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        var genericPaginationResponse = new GenericPaginationResponse<Domain.Entities.Notification>();

        var normalizedText = genericSearchPaginationRequest.Texto?.NormalizeString();

        var query =
            _repository?.Where(x => string.IsNullOrEmpty(normalizedText) ||
                                    (
                                      x.Order.Description.ToUpper().Trim().Contains(normalizedText)
                                    ));

        if (genericSearchPaginationRequest.Id.HasValue)
            query = _repository?.Where(x => x.Id == genericSearchPaginationRequest.Id.Value);

        if (genericSearchPaginationRequest.ListaRelacionamento != null &&
            genericSearchPaginationRequest.ListaRelacionamento.Any())
        {
            foreach (var relational in genericSearchPaginationRequest.ListaRelacionamento)
            {
                if (relational.Item1.ToUpper() == SystemConst.FieldOrderId.ToUpper())
                    query = query.Where(c => c.OrderId == relational.Item2);
            }
        }

        query =
            query.Where(x => (!genericSearchPaginationRequest.DataInicio.HasValue || x.Date >= genericSearchPaginationRequest.DataInicio.Value) &&
                             (!genericSearchPaginationRequest.DataFim.HasValue || x.Date <= genericSearchPaginationRequest.DataFim.Value));

        if (!string.IsNullOrEmpty(genericSearchPaginationRequest.CampoOrdenacao))
            query = query?.OrderBy($"{genericSearchPaginationRequest.CampoOrdenacao} {genericSearchPaginationRequest.DirecaoOrdenacao}");

        query = query?.Include(c => c.Order)
                     ?.Select(x => x);

        genericPaginationResponse.LoadPagination(query, genericSearchPaginationRequest);

        if (genericPaginationResponse == null || genericPaginationResponse.Total == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.NotificationNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(genericPaginationResponse);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Notification>> outputPort)
        => _outputPort = outputPort;
}