using Application.UseCases.V1.Plan.DeletePlan.Interfaces;
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Plan;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/planos", Name = "planos")]
[ApiController]
public class DeletePlanController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.Plan>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public DeletePlanController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<DeletePlanResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<DeletePlanResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<DeletePlanResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Delete))]
    [SwaggerOperation(Summary = "Remover um plano por id")]
    public async Task<IActionResult> DeletePlan(
        [FromServices] IDeletePlanUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.Plan>.Ok(
        Domain.Entities.Plan plan)
        => _viewModel = base.Ok(new GenericResponse<DeletePlanResponse>(true, new DeletePlanResponse(plan), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.Plan>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<DeletePlanResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.Plan>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<DeletePlanResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}