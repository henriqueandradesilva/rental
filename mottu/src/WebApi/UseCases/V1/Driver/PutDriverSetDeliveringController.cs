using Application.UseCases.V1.Driver.PutDriverSetDelivering.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Driver.Request;
using CrossCutting.Dtos.Driver.Response;
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

namespace WebApi.UseCases.V1.Driver;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/entregadores", Name = "entregadores")]
[ApiController]
public class PutDriverSetDeliveringController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Driver>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutDriverSetDeliveringController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}/entregando")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutDriverResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutDriverResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Modificar o status de um pedido")]
    public async Task<IActionResult> PutDriverSetDelivering(
        [FromServices] IPutDriverSetDeliveringUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutDriverSetDeliveringRequest putDriverSetDeliveringRequest)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id, putDriverSetDeliveringRequest.Entregando);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Driver>.Ok(
        Domain.Entities.Driver driver)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.DriverUpdated);

        _viewModel = base.Ok(new GenericResponse<PutDriverResponse>(true, new PutDriverResponse(driver), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.Driver>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutDriverResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}