using Application.UseCases.V1.DriverNotificated.GetListSearchDriverNotificated.Interfaces;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.DriverNotificated.Response;
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

namespace WebApi.UseCases.V1.DriverNotificated;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/entregadoresNotificados", Name = "entregadoresNotificados")]
[ApiController]
public class GetListSearchDriverNotificatedController : CustomControllerBaseExtension, IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.DriverNotificated>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchDriverNotificatedController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost("consultar")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericPaginationResponse<GetListDriverNotificatedResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericPaginationResponse<GetListDriverNotificatedResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericPaginationResponse<GetListDriverNotificatedResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Consultar entregadores notificados com paginação")]
    public IActionResult GetListSearchDriverNotificated(
       [FromServices] IGetListSearchDriverNotificatedUseCase useCase,
       [FromBody] GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        useCase.SetOutputPort(this);

        useCase.Execute(genericSearchPaginationRequest);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.DriverNotificated>>.Ok(
        GenericPaginationResponse<Domain.Entities.DriverNotificated> genericPaginationResponse)
        => _viewModel = base.Ok(new GenericPaginationResponse<GetListDriverNotificatedResponse>(true, new GetListDriverNotificatedResponse().GetListDriverNotificated(genericPaginationResponse.ListaResultado), genericPaginationResponse.Total, null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.DriverNotificated>>.Error()
        => _viewModel = base.BadRequest(new GenericPaginationResponse<GetListDriverNotificatedResponse>(false, null, 0, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.DriverNotificated>>.NotFound()
        => _viewModel = base.NotFound(new GenericPaginationResponse<GetListDriverNotificatedResponse>(true, null, 0, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}