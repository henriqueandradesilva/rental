using Application.UseCases.V1.Notification.PutNotification.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Notification.Request;
using CrossCutting.Dtos.Notification.Response;
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

namespace WebApi.UseCases.V1.Notification;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/notificacoes", Name = "notificacoes")]
[ApiController]
public class PutNotificationController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Notification>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutNotificationController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutNotificationResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutNotificationResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar um tipo de status de pedido por id")]
    public async Task<IActionResult> PutNotification(
        [FromServices] IPutNotificationUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutNotificationRequest putNotificationRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.Notification Notification =
            new Domain.Entities.Notification(
            id,
            putNotificationRequest.PedidoId,
            putNotificationRequest.Data);

        await useCase.Execute(Notification);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Notification>.Ok(
        Domain.Entities.Notification notification)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.NotificationUpdated);

        _viewModel = base.Ok(new GenericResponse<PutNotificationResponse>(true, new PutNotificationResponse(notification), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.Notification>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutNotificationResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}