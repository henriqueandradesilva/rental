using Application.UseCases.V1.DriverNotificated.GetListAllDriverNotificated.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.DriverNotificated.Response;
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

namespace WebApi.UseCases.V1.DriverNotificated;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/entregadoresNotificados", Name = "entregadoresNotificados")]
[ApiController]
public class GetListAllDriverNotificatedController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<Domain.Entities.DriverNotificated>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllDriverNotificatedController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("listar")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListDriverNotificatedResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListDriverNotificatedResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListDriverNotificatedResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Listar todos os entregadores notificados")]
    public async Task<IActionResult> GetListDriverNotificated(
        [FromServices] IGetListAllDriverNotificatedUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<Domain.Entities.DriverNotificated>>.Ok(
        List<Domain.Entities.DriverNotificated> listDriverNotificated)
        => _viewModel = base.Ok(new GenericResponse<List<GetListDriverNotificatedResponse>>(true, new GetListDriverNotificatedResponse().GetListDriverNotificated(listDriverNotificated), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<Domain.Entities.DriverNotificated>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListDriverNotificatedResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<Domain.Entities.DriverNotificated>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListDriverNotificatedResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}