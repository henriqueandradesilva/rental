using Application.UseCases.V1.PlanType.DeletePlanType.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.PlanType.Response;
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

namespace WebApi.UseCases.V1.PlanType;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tiposPlano", Name = "tiposPlano")]
[ApiController]
public class DeletePlanTypeController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.PlanType>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public DeletePlanTypeController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<DeletePlanTypeResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<DeletePlanTypeResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<DeletePlanTypeResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Delete))]
    [SwaggerOperation(Summary = "Remover um tipo de plano por id")]
    public async Task<IActionResult> DeletePlanType(
        [FromServices] IDeletePlanTypeUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.PlanType>.Ok(
        Domain.Entities.PlanType planType)
        => _viewModel = base.Ok(new GenericResponse<DeletePlanTypeResponse>(true, new DeletePlanTypeResponse(planType), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.PlanType>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<DeletePlanTypeResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.PlanType>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<DeletePlanTypeResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}