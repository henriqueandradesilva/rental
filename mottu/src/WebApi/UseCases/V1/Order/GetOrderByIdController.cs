using Application.UseCases.V1.Order.GetOrderById.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Order.Response;
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

namespace WebApi.UseCases.V1.Order;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pedidos", Name = "pedidos")]
[ApiController]
public class GetOrderByIdController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.Order>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetOrderByIdController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<GetOrderResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<GetOrderResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<GetOrderResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Get))]
    [SwaggerOperation(Summary = "Consultar um pedido por id")]
    public async Task<IActionResult> GetOrder(
        [FromServices] IGetOrderByIdUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.Order>.Ok(
        Domain.Entities.Order order)
        => _viewModel = base.Ok(new GenericResponse<GetOrderResponse>(true, new GetOrderResponse().GetOrder(order), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.Order>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<GetOrderResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.Order>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<GetOrderResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}