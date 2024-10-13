using Application.UseCases.V1.OrderStatus.GetOrderStatusById.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.OrderStatus.Response;
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

namespace WebApi.UseCases.V1.OrderStatus;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/statusPedido", Name = "statusPedido")]
[ApiController]
public class GetOrderStatusByIdController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.OrderStatus>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetOrderStatusByIdController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<GetOrderStatusResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<GetOrderStatusResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<GetOrderStatusResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Get))]
    [SwaggerOperation(Summary = "Consultar um tipo de status de pedido por id")]
    public async Task<IActionResult> GetOrderStatus(
        [FromServices] IGetOrderStatusByIdUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.OrderStatus>.Ok(
        Domain.Entities.OrderStatus orderStatus)
        => _viewModel = base.Ok(new GenericResponse<GetOrderStatusResponse>(true, new GetOrderStatusResponse().GetOrderStatus(orderStatus), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.OrderStatus>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<GetOrderStatusResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.OrderStatus>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<GetOrderStatusResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}