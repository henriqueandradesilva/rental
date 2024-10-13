using Application.UseCases.V1.OrderDelivered.DeleteOrderDelivered.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.OrderDelivered.Response;
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

namespace WebApi.UseCases.V1.OrderDelivered;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pedidosEntregues", Name = "pedidosEntregues")]
[ApiController]
public class DeleteOrderDeliveredController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.OrderDelivered>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public DeleteOrderDeliveredController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<DeleteOrderDeliveredResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<DeleteOrderDeliveredResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<DeleteOrderDeliveredResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Delete))]
    [SwaggerOperation(Summary = "Remover um pedido entregue por id")]
    public async Task<IActionResult> DeleteOrderDelivered(
        [FromServices] IDeleteOrderDeliveredUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.OrderDelivered>.Ok(
        Domain.Entities.OrderDelivered orderDelivered)
        => _viewModel = base.Ok(new GenericResponse<DeleteOrderDeliveredResponse>(true, new DeleteOrderDeliveredResponse(orderDelivered), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.OrderDelivered>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<DeleteOrderDeliveredResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.OrderDelivered>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<DeleteOrderDeliveredResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}