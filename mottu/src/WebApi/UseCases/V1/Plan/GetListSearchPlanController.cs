using Application.UseCases.V1.Plan.GetListSearchPlan.Interfaces;
using CrossCutting.Common.Dtos.Request;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Plan.Response;
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

namespace WebApi.UseCases.V1.Plan;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/planos", Name = "planos")]
[ApiController]
public class GetListSearchPlanController : CustomControllerBaseExtension, IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Plan>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListSearchPlanController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost("consultar")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericPaginationResponse<GetListPlanResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericPaginationResponse<GetListPlanResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericPaginationResponse<GetListPlanResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Consultar planos com paginação")]
    public IActionResult GetListSearchPlan(
       [FromServices] IGetListSearchPlanUseCase useCase,
       [FromBody] GenericSearchPaginationRequest genericSearchPaginationRequest)
    {
        useCase.SetOutputPort(this);

        useCase.Execute(genericSearchPaginationRequest);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Plan>>.Ok(
        GenericPaginationResponse<Domain.Entities.Plan> genericPaginationResponse)
        => _viewModel = base.Ok(new GenericPaginationResponse<GetListPlanResponse>(true, new GetListPlanResponse().GetListPlan(genericPaginationResponse.ListaResultado), genericPaginationResponse.Total, null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Plan>>.Error()
        => _viewModel = base.BadRequest(new GenericPaginationResponse<GetListPlanResponse>(false, null, 0, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<GenericPaginationResponse<Domain.Entities.Plan>>.NotFound()
        => _viewModel = base.NotFound(new GenericPaginationResponse<GetListPlanResponse>(true, null, 0, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}