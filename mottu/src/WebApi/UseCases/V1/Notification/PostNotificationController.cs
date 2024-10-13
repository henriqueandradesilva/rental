using Application.UseCases.V1.Notification.PostNotification.Interfaces;
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
public class PostNotificationController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Notification>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostNotificationController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostNotificationResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostNotificationResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar uma nova notificação")]
    public async Task<IActionResult> PostNotification(
        [FromServices] IPostNotificationUseCase useCase,
        [FromBody][Required] PostNotificationRequest postNotificationRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.Notification Notification =
            new Domain.Entities.Notification(
            0,
            postNotificationRequest.PedidoId,
            postNotificationRequest.Data);

        await useCase.Execute(Notification);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Notification>.Ok(
        Domain.Entities.Notification notification)
    {
        var uri = $"/notificacoes/{notification.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.NotificationCreated);

        var response =
            new GenericResponse<PostNotificationResponse>(true, new PostNotificationResponse(notification), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.Notification>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostNotificationResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}