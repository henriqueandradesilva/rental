using Application.UseCases.V1.Order.GetListSearchOrder.Interfaces;
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

namespace Application.UseCases.V1.Order.GetListSearchOrder;

public class GetListSearchOrderUseCase : IGetListSearchOrderUseCase
{
    private IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Order>> _outputPort;
    private IOrderRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchOrderUseCase(
        IOrderRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest)
    {

        var genericPaginationResponse = new GenericPaginationResponse<Domain.Entities.Order>();

        var normalizedText = genericSearchPaginationRequest.Texto?.NormalizeString();

        var query =
            _repository?.Where(x => string.IsNullOrEmpty(normalizedText) ||
                                    (
                                      x.Description.ToUpper().Trim().Contains(normalizedText) ||
                                      x.Status.Description.ToUpper().Trim().Contains(normalizedText)
                                    ));

        if (genericSearchPaginationRequest.Id.HasValue)
            query = _repository?.Where(x => x.Id == genericSearchPaginationRequest.Id.Value);

        if (genericSearchPaginationRequest.ListaRelacionamento != null &&
            genericSearchPaginationRequest.ListaRelacionamento.Any())
        {
            foreach (var relational in genericSearchPaginationRequest.ListaRelacionamento)
            {
                if (relational.Item1.ToUpper() == SystemConst.FieldStatusId.ToUpper())
                    query = query.Where(c => c.StatusId == relational.Item2);
            }
        }

        query =
            query.Where(x => (!genericSearchPaginationRequest.DataInicio.HasValue || x.Date >= genericSearchPaginationRequest.DataInicio.Value) &&
                             (!genericSearchPaginationRequest.DataFim.HasValue || x.Date <= genericSearchPaginationRequest.DataFim.Value));

        if (!string.IsNullOrEmpty(genericSearchPaginationRequest.CampoOrdenacao))
            query = query?.OrderBy($"{genericSearchPaginationRequest.CampoOrdenacao} {genericSearchPaginationRequest.DirecaoOrdenacao}");

        query = query?.Include(c => c.Status)
                     ?.Include(c => c.Accepted)
                     ?.Include(c => c.Delivered)
                     ?.Select(x => x);

        genericPaginationResponse.LoadPagination(query, genericSearchPaginationRequest);

        if (genericPaginationResponse == null || genericPaginationResponse.Total == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.OrderNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(genericPaginationResponse);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Order>> outputPort)
        => _outputPort = outputPort;
}