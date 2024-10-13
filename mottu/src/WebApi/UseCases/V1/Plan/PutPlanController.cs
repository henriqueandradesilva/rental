using Application.UseCases.V1.Plan.PutPlan.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Plan.Request;
using CrossCutting.Dtos.Plan.Response;
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

namespace WebApi.UseCases.V1.Plan;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/planos", Name = "planos")]
[ApiController]
public class PutPlanController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Plan>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutPlanController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutPlanResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutPlanResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar um plano por id")]
    public async Task<IActionResult> PutPlan(
        [FromServices] IPutPlanUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutPlanRequest putPlanRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.Plan plan =
            new Domain.Entities.Plan(
            id,
            putPlanRequest.TipoPlanoId,
            putPlanRequest.Descricao,
            putPlanRequest.TaxaDiaria,
            putPlanRequest.TaxaAdicional,
            putPlanRequest.TaxaFixa,
            putPlanRequest.DuracaoEmDias,
            putPlanRequest.Ativo);

        await useCase.Execute(plan);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Plan>.Ok(
        Domain.Entities.Plan plan)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.PlanUpdated);

        _viewModel = base.Ok(new GenericResponse<PutPlanResponse>(true, new PutPlanResponse(plan), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.Plan>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutPlanResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}