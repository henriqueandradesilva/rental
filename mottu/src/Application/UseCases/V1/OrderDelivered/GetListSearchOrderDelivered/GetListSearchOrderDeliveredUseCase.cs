using Application.UseCases.V1.OrderDelivered.GetListSearchOrderDelivered.Interfaces;
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

namespace Application.UseCases.V1.OrderDelivered.GetListSearchOrderDelivered;

public class GetListSearchOrderDeliveredUseCase : IGetListSearchOrderDeliveredUseCase
{
    private IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.OrderDelivered>> _outputPort;
    private readonly IOrderDeliveredRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchOrderDeliveredUseCase(
        IOrderDeliveredRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        var genericPaginationResponse = new GenericPaginationResponse<Domain.Entities.OrderDelivered>();

        var normalizedText = genericSearchPaginationRequest.Texto?.NormalizeString();

        var query =
            _repository?.Where(x => string.IsNullOrEmpty(normalizedText) ||
                                    (
                                      x.Driver.Identifier.ToUpper().Trim().Contains(normalizedText) ||
                                      x.Driver.Name.ToUpper().Trim().Contains(normalizedText) ||
                                      x.Driver.Cnpj.ToUpper().Trim().Contains(normalizedText) ||
                                      x.Driver.Cnh.ToUpper().Trim().Contains(normalizedText) ||
                                      x.Order.Description.ToUpper().Trim().Contains(normalizedText) ||
                                      x.Order.Status.Description.ToUpper().Trim().Contains(normalizedText)
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

        query = query?.Include(c => c.Driver)
                     ?.Include(c => c.Order)
                     ?.Select(x => x);

        genericPaginationResponse.LoadPagination(query, genericSearchPaginationRequest);

        if (genericPaginationResponse == null || genericPaginationResponse.Total == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.OrderDeliveredNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(genericPaginationResponse);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.OrderDelivered>> outputPort)
        => _outputPort = outputPort;
}