using Application.UseCases.V1.UserRole.GetListSearchUserRole.Interfaces;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Application.UseCases.V1.UserRole.GetListSearchUserRole;

public class GetListSearchUserRoleUseCase : IGetListSearchUserRoleUseCase
{
    private IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.UserRole>> _outputPort;
    private readonly IUserRoleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchUserRoleUseCase(
        IUserRoleRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        var genericPaginationResponse = new GenericPaginationResponse<Domain.Entities.UserRole>();

        var normalizedText = genericSearchPaginationRequest.Texto?.NormalizeString();

        var query =
            _repository?.Where(x => string.IsNullOrEmpty(normalizedText) ||
                                    (
                                     x.Description.ToUpper().Trim().Contains(normalizedText)
                                    ));

        if (genericSearchPaginationRequest.Id.HasValue)
            query = _repository?.Where(x => x.Id == genericSearchPaginationRequest.Id.Value);

        query = DateFilterExtension<Domain.Entities.UserRole>.Filter(genericSearchPaginationRequest, query);

        if (!string.IsNullOrEmpty(genericSearchPaginationRequest.CampoOrdenacao))
            query = query?.OrderBy($"{genericSearchPaginationRequest.CampoOrdenacao} {genericSearchPaginationRequest.DirecaoOrdenacao}");

        query = query?.Select(x => x);

        genericPaginationResponse.LoadPagination(query, genericSearchPaginationRequest);

        if (genericPaginationResponse == null || genericPaginationResponse.Total == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.UserRoleNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(genericPaginationResponse);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.UserRole>> outputPort)
        => _outputPort = outputPort;
}