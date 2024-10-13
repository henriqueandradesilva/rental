using Application.UseCases.V1.Plan.GetListAllPlan.Interfaces;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Plan;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/planos", Name = "planos")]
[ApiController]
public class GetListAllPlanController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<Domain.Entities.Plan>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllPlanController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("listar")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListPlanResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListPlanResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListPlanResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Listar todos os planos")]
    public async Task<IActionResult> GetListPlan(
        [FromServices] IGetListAllPlanUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<Domain.Entities.Plan>>.Ok(
        List<Domain.Entities.Plan> listPlan)
        => _viewModel = base.Ok(new GenericResponse<List<GetListPlanResponse>>(true, new GetListPlanResponse().GetListPlan(listPlan), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<Domain.Entities.Plan>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListPlanResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<Domain.Entities.Plan>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListPlanResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}