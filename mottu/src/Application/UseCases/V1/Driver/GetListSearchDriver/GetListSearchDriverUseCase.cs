using Application.UseCases.V1.Driver.GetListSearchDriver.Interfaces;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Common.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Application.UseCases.V1.Driver.GetListSearchDriver;

public class GetListSearchDriverUseCase : IGetListSearchDriverUseCase
{
    private IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Driver>> _outputPort;
    private readonly IDriverRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchDriverUseCase(
        IDriverRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        var genericPaginationResponse = new GenericPaginationResponse<Domain.Entities.Driver>();

        var normalizedText = genericSearchPaginationRequest.Texto?.NormalizeString();

        var query =
            _repository?.Where(x => string.IsNullOrEmpty(normalizedText) ||
                                    (
                                     x.Identifier.ToUpper().Trim().Contains(normalizedText) ||
                                     x.Name.ToUpper().Trim().Contains(normalizedText) ||
                                     x.Cnpj.ToUpper().Trim().Contains(normalizedText) ||
                                     x.Cnh.ToUpper().Trim().Contains(normalizedText) ||
                                     x.User.Name.ToUpper().Trim().Contains(normalizedText) ||
                                     x.User.Email.ToUpper().Trim().Contains(normalizedText)
                                    ));

        if (genericSearchPaginationRequest.Id.HasValue)
            query = _repository?.Where(x => x.Id == genericSearchPaginationRequest.Id.Value);

        if (genericSearchPaginationRequest.ListaRelacionamento != null &&
            genericSearchPaginationRequest.ListaRelacionamento.Any())
        {
            foreach (var relational in genericSearchPaginationRequest.ListaRelacionamento)
            {
                if (relational.Item1.ToUpper() == SystemConst.FieldUserId.ToUpper())
                    query = query.Where(c => c.UserId == relational.Item2);
            }
        }

        if (genericSearchPaginationRequest.ListaEnum != null &&
            genericSearchPaginationRequest.ListaEnum.Any())
        {
            foreach (var enumerator in genericSearchPaginationRequest.ListaEnum)
            {
                if (enumerator.Item1.ToUpper() == SystemConst.FieldCnhType.ToUpper())
                    query = query.Where(c => c.Type == (CnhTypeEnum)enumerator.Item2);
            }
        }

        query =
            query.Where(x => (!genericSearchPaginationRequest.DataInicio.HasValue || x.DateOfBirth >= DateOnly.FromDateTime(genericSearchPaginationRequest.DataInicio.Value)) &&
                             (!genericSearchPaginationRequest.DataFim.HasValue || x.DateOfBirth <= DateOnly.FromDateTime(genericSearchPaginationRequest.DataFim.Value)));

        if (!string.IsNullOrEmpty(genericSearchPaginationRequest.CampoOrdenacao))
            query = query?.OrderBy($"{genericSearchPaginationRequest.CampoOrdenacao} {genericSearchPaginationRequest.DirecaoOrdenacao}");

        query = query?.Include(c => c.User)
                     ?.Select(x => x);

        genericPaginationResponse.LoadPagination(query, genericSearchPaginationRequest);

        if (genericPaginationResponse == null || genericPaginationResponse.Total == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.DriverNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(genericPaginationResponse);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Driver>> outputPort)
        => _outputPort = outputPort;
}