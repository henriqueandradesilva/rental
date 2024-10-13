using Application.UseCases.V1.DriverNotificated.GetDriverNotificatedById.Interfaces;
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.DriverNotificated;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/entregadoresNotificados", Name = "entregadoresNotificados")]
[ApiController]
public class GetDriverNotificatedByIdController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.DriverNotificated>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetDriverNotificatedByIdController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<GetDriverNotificatedResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<GetDriverNotificatedResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<GetDriverNotificatedResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Get))]
    [SwaggerOperation(Summary = "Consultar um entregador notificado por id")]
    public async Task<IActionResult> GetDriverNotificated(
        [FromServices] IGetDriverNotificatedByIdUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.DriverNotificated>.Ok(
        Domain.Entities.DriverNotificated driverNotificated)
        => _viewModel = base.Ok(new GenericResponse<GetDriverNotificatedResponse>(true, new GetDriverNotificatedResponse().GetDriverNotificated(driverNotificated), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.DriverNotificated>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<GetDriverNotificatedResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.DriverNotificated>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<GetDriverNotificatedResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}