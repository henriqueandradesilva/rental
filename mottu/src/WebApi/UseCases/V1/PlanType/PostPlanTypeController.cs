using Application.UseCases.V1.PlanType.PostPlanType.Interfaces;
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
public class PostPlanTypeController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.PlanType>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostPlanTypeController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostPlanTypeResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostPlanTypeResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar um novo tipo de plano")]
    public async Task<IActionResult> PostPlanType(
        [FromServices] IPostPlanTypeUseCase useCase,
        [FromBody][Required] PostPlanTypeRequest postPlanTypeRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.PlanType planType =
            new Domain.Entities.PlanType(
            0,
            postPlanTypeRequest.Descricao);

        await useCase.Execute(planType);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.PlanType>.Ok(
        Domain.Entities.PlanType planType)
    {
        var uri = $"/tiposPlano/{planType.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.PlanTypeCreated);

        var response =
            new GenericResponse<PostPlanTypeResponse>(true, new PostPlanTypeResponse(planType), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.PlanType>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostPlanTypeResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}