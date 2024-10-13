using Application.UseCases.V1.Motorcycle.PutMotorcycleSetRented.Interfaces;
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
public class PutMotorcycleSetRentedController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Motorcycle>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutMotorcycleSetRentedController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}/alugado")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutMotorcycleResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutMotorcycleResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Modificar campo alugado de uma moto por id")]
    public async Task<IActionResult> PutMotorcycleSetRented(
        [FromServices] IPutMotorcycleSetRentedUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutMotorcycleSetRentedRequest putMotorcycleSetRentedRequest)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id, putMotorcycleSetRentedRequest.Alugado);

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