using Application.UseCases.V1.DriverNotificated.PostDriverNotificated.Interfaces;
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
public class PostDriverNotificatedController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.DriverNotificated>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostDriverNotificatedController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostDriverNotificatedResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostDriverNotificatedResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar um entregador notificado")]
    public async Task<IActionResult> PostDriverNotificated(
        [FromServices] IPostDriverNotificatedUseCase useCase,
        [FromBody][Required] PostDriverNotificatedRequest postDriverNotificatedRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.DriverNotificated driverNotificated =
            new Domain.Entities.DriverNotificated(
            0,
            postDriverNotificatedRequest.EntregadorId,
            postDriverNotificatedRequest.NotificacaoId,
            postDriverNotificatedRequest.Data);

        await useCase.Execute(driverNotificated);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.DriverNotificated>.Ok(
        Domain.Entities.DriverNotificated driverNotificated)
    {
        var uri = $"/entregadoresNotificados/{driverNotificated.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.DriverNotificated);

        var response =
            new GenericResponse<PostDriverNotificatedResponse>(true, new PostDriverNotificatedResponse(driverNotificated), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.DriverNotificated>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostDriverNotificatedResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}