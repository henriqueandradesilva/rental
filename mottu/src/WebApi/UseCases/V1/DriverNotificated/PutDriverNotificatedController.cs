using Application.UseCases.V1.DriverNotificated.PutDriverNotificated.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.DriverNotificated.Request;
using CrossCutting.Dtos.DriverNotificated.Response;
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

namespace WebApi.UseCases.V1.DriverNotificated;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/entregadoresNotificados", Name = "entregadoresNotificados")]
[ApiController]
public class PutDriverNotificatedController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.DriverNotificated>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutDriverNotificatedController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutDriverNotificatedResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutDriverNotificatedResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar um entregador notificado por id")]
    public async Task<IActionResult> PutDriverNotificated(
        [FromServices] IPutDriverNotificatedUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutDriverNotificatedRequest putDriverNotificatedRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.DriverNotificated driverNotificated =
            new Domain.Entities.DriverNotificated(
            id,
            putDriverNotificatedRequest.EntregadorId,
            putDriverNotificatedRequest.NotificacaoId,
            putDriverNotificatedRequest.Data);

        await useCase.Execute(driverNotificated);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.DriverNotificated>.Ok(
        Domain.Entities.DriverNotificated driverNotificated)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.DriverNotificated);

        _viewModel = base.Ok(new GenericResponse<PutDriverNotificatedResponse>(true, new PutDriverNotificatedResponse(driverNotificated), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.DriverNotificated>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutDriverNotificatedResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}