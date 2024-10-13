using Application.UseCases.V1.Motorcycle.PutMotorcycle.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Motorcycle.Request;
using CrossCutting.Dtos.Motorcycle.Response;
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

namespace WebApi.UseCases.V1.Motorcycle;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/motos", Name = "motos")]
[ApiController]
public class PutMotorcycleController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Motorcycle>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutMotorcycleController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutMotorcycleResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutMotorcycleResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar uma moto por id")]
    public async Task<IActionResult> PutMotorcycle(
        [FromServices] IPutMotorcycleUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutMotorcycleRequest putMotorcycleRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.Motorcycle motorcycle =
            new Domain.Entities.Motorcycle(
            id,
            putMotorcycleRequest.Identificador,
            putMotorcycleRequest.Ano,
            putMotorcycleRequest.Placa);

        await useCase.Execute(motorcycle, putMotorcycleRequest.Modelo);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Motorcycle>.Ok(
        Domain.Entities.Motorcycle motorcycle)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.MotorcycleUpdated);

        _viewModel = base.Ok(new GenericResponse<PutMotorcycleResponse>(true, new PutMotorcycleResponse(motorcycle), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.Motorcycle>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutMotorcycleResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}