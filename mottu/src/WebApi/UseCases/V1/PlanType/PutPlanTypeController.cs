using Application.UseCases.V1.PlanType.PutPlanType.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.PlanType.Request;
using CrossCutting.Dtos.PlanType.Response;
using CrossCutting.Enums;
using CrossCutting.Extensions.Api;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.PlanType;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tiposPlano", Name = "tiposPlano")]
[ApiController]
public class PutPlanTypeController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.PlanType>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutPlanTypeController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutPlanTypeResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutPlanTypeResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar um tipo de plano por id")]
    public async Task<IActionResult> PutPlanType(
        [FromServices] IPutPlanTypeUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutPlanTypeRequest putPlanTypeRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.PlanType planType =
            new Domain.Entities.PlanType(
            id,
            putPlanTypeRequest.Descricao);

        await useCase.Execute(planType);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.PlanType>.Ok(
        Domain.Entities.PlanType planType)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.PlanTypeUpdated);

        _viewModel = base.Ok(new GenericResponse<PutPlanTypeResponse>(true, new PutPlanTypeResponse(planType), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.PlanType>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutPlanTypeResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}