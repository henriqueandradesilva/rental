using Application.UseCases.V1.Plan.PostPlan.Interfaces;
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
public class PostPlanController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Plan>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostPlanController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostPlanResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostPlanResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar um plano")]
    public async Task<IActionResult> PostPlan(
        [FromServices] IPostPlanUseCase useCase,
        [FromBody][Required] PostPlanRequest postPlanRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.Plan plan =
            new Domain.Entities.Plan(
            0,
            postPlanRequest.TipoPlanoId,
            postPlanRequest.Descricao,
            postPlanRequest.TaxaDiaria,
            postPlanRequest.TaxaAdicional,
            postPlanRequest.TaxaFixa,
            postPlanRequest.DuracaoEmDias,
            postPlanRequest.Ativo);

        await useCase.Execute(plan);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Plan>.Ok(
        Domain.Entities.Plan plan)
    {
        var uri = $"/planos/{plan.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.PlanCreated);

        var response =
            new GenericResponse<PostPlanResponse>(true, new PostPlanResponse(plan), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.Plan>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostPlanResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}