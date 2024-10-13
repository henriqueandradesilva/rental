using Application.UseCases.V1.Notification.DeleteNotification.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Notification.Response;
using CrossCutting.Enums;
using CrossCutting.Extensions.Api;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Notification;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/notificacoes", Name = "notificacoes")]
[ApiController]
public class DeleteNotificationController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.Notification>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public DeleteNotificationController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<DeleteNotificationResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<DeleteNotificationResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<DeleteNotificationResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Delete))]
    [SwaggerOperation(Summary = "Remover uma notificação por id")]
    public async Task<IActionResult> DeleteNotification(
        [FromServices] IDeleteNotificationUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.Notification>.Ok(
        Domain.Entities.Notification notification)
        => _viewModel = base.Ok(new GenericResponse<DeleteNotificationResponse>(true, new DeleteNotificationResponse(notification), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.Notification>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<DeleteNotificationResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.Notification>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<DeleteNotificationResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}