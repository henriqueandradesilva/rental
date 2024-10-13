using Application.UseCases.V1.ModelVehicle.PutModelVehicle.Interfaces;
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
public class PutModelVehicleController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.ModelVehicle>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutModelVehicleController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutModelVehicleResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutModelVehicleResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar um modelo de veículo por id")]
    public async Task<IActionResult> PutModelVehicle(
        [FromServices] IPutModelVehicleUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutModelVehicleRequest putModelVehicleRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.ModelVehicle modelVehicle =
            new Domain.Entities.ModelVehicle(
            id,
            putModelVehicleRequest.Descricao);

        await useCase.Execute(modelVehicle);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.ModelVehicle>.Ok(
        Domain.Entities.ModelVehicle modelVehicle)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.ModelVehicleUpdated);

        _viewModel = base.Ok(new GenericResponse<PutModelVehicleResponse>(true, new PutModelVehicleResponse(modelVehicle), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.ModelVehicle>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutModelVehicleResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}