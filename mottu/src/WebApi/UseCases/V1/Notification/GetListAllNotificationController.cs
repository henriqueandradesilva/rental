using Application.UseCases.V1.Notification.GetListAllNotification.Interfaces;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Notification;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/notificacoes", Name = "notificacoes")]
[ApiController]
public class GetListAllNotificationController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<Domain.Entities.Notification>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllNotificationController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("listar")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListNotificationResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListNotificationResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListNotificationResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Listar todas as notificações")]
    public async Task<IActionResult> GetListNotification(
        [FromServices] IGetListAllNotificationUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<Domain.Entities.Notification>>.Ok(
        List<Domain.Entities.Notification> listNotification)
        => _viewModel = base.Ok(new GenericResponse<List<GetListNotificationResponse>>(true, new GetListNotificationResponse().GetListNotification(listNotification), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<Domain.Entities.Notification>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListNotificationResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<Domain.Entities.Notification>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListNotificationResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}