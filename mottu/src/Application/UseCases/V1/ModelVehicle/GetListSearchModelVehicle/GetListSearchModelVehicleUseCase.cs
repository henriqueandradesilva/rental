using Application.UseCases.V1.ModelVehicle.GetListSearchModelVehicle.Interfaces;
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

namespace Application.UseCases.V1.ModelVehicle.GetListSearchModelVehicle;

public class GetListSearchModelVehicleUseCase : IGetListSearchModelVehicleUseCase
{
    private IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.ModelVehicle>> _outputPort;
    private readonly IModelVehicleRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchModelVehicleUseCase(
        IModelVehicleRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public void Execute(
        GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        var genericPaginationResponse = new GenericPaginationResponse<Domain.Entities.ModelVehicle>();

        var normalizedText = genericSearchPaginationRequest.Texto?.NormalizeString();

        var query =
            _repository?.Where(x => string.IsNullOrEmpty(normalizedText) ||
                                    (
                                     x.Description.ToUpper().Trim().Contains(normalizedText)
                                    ));

        if (genericSearchPaginationRequest.Id.HasValue)
            query = _repository?.Where(x => x.Id == genericSearchPaginationRequest.Id.Value);

        query = DateFilterExtension<Domain.Entities.ModelVehicle>.Filter(genericSearchPaginationRequest, query);

        if (!string.IsNullOrEmpty(genericSearchPaginationRequest.CampoOrdenacao))
            query = query?.OrderBy($"{genericSearchPaginationRequest.CampoOrdenacao} {genericSearchPaginationRequest.DirecaoOrdenacao}");

        query = query?.Select(x => x);

        genericPaginationResponse.LoadPagination(query, genericSearchPaginationRequest);

        if (genericPaginationResponse == null || genericPaginationResponse.Total == 0)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.ModelVehicleNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(genericPaginationResponse);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.ModelVehicle>> outputPort)
        => _outputPort = outputPort;
}