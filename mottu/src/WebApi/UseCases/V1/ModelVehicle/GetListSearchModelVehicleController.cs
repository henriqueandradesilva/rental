using Application.UseCases.V1.ModelVehicle.GetListSearchModelVehicle.Interfaces;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.ModelVehicle.Response;
using CrossCutting.Enums;
using CrossCutting.Extensions.Api;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;

namespace WebApi.UseCases.V1.ModelVehicle;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/modelosVeiculo", Name = "modelosVeiculo")]
[ApiController]
public class GetListSearchModelVehicleController : CustomControllerBaseExtension, IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.ModelVehicle>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchModelVehicleController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost("consultar")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericPaginationResponse<GetListModelVehicleResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericPaginationResponse<GetListModelVehicleResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericPaginationResponse<GetListModelVehicleResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Consultar modelos de veículos com paginação")]
    public IActionResult GetListSearchModelVehicle(
       [FromServices] IGetListSearchModelVehicleUseCase useCase,
       [FromBody] GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        useCase.SetOutputPort(this);

        useCase.Execute(genericSearchPaginationRequest);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.ModelVehicle>>.Ok(
        GenericPaginationResponse<Domain.Entities.ModelVehicle> genericPaginationResponse)
        => _viewModel = base.Ok(new GenericPaginationResponse<GetListModelVehicleResponse>(true, new GetListModelVehicleResponse().GetListModelVehicle(genericPaginationResponse.ListaResultado), genericPaginationResponse.Total, null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.ModelVehicle>>.Error()
        => _viewModel = base.BadRequest(new GenericPaginationResponse<GetListModelVehicleResponse>(false, null, 0, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.ModelVehicle>>.NotFound()
        => _viewModel = base.NotFound(new GenericPaginationResponse<GetListModelVehicleResponse>(true, null, 0, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}