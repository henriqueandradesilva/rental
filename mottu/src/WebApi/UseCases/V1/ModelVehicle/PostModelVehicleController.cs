using Application.UseCases.V1.ModelVehicle.PostModelVehicle.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.ModelVehicle.Request;
using CrossCutting.Dtos.ModelVehicle.Response;
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

namespace WebApi.UseCases.V1.ModelVehicle;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/modelosVeiculo", Name = "modelosVeiculo")]
[ApiController]
public class PostModelVehicleController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.ModelVehicle>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostModelVehicleController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostModelVehicleResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostModelVehicleResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar um novo modelo de veículo")]
    public async Task<IActionResult> PostModelVehicle(
        [FromServices] IPostModelVehicleUseCase useCase,
        [FromBody][Required] PostModelVehicleRequest postModelVehicleRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.ModelVehicle modelVehicle =
            new Domain.Entities.ModelVehicle(
            0,
            postModelVehicleRequest.Descricao);

        await useCase.Execute(modelVehicle);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.ModelVehicle>.Ok(
        Domain.Entities.ModelVehicle modelVehicle)
    {
        var uri = $"/modelosVeiculo/{modelVehicle.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.ModelVehicleCreated);

        var response =
            new GenericResponse<PostModelVehicleResponse>(true, new PostModelVehicleResponse(modelVehicle), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.ModelVehicle>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostModelVehicleResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}