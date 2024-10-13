using Application.UseCases.V1.Order.PutOrder.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Order.Request;
using CrossCutting.Dtos.Order.Response;
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

namespace WebApi.UseCases.V1.Order;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pedidos", Name = "pedidos")]
[ApiController]
public class PutOrderController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Order>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutOrderController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutOrderResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutOrderResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar um pedido por id")]
    public async Task<IActionResult> PutOrder(
        [FromServices] IPutOrderUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutOrderRequest putOrderRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.Order order =
            new Domain.Entities.Order(
            id,
            SystemConst.OrderStatusAvailableDefault,
            putOrderRequest.Descricao,
            putOrderRequest.Valor,
            putOrderRequest.Data);

        await useCase.Execute(order);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Order>.Ok(
        Domain.Entities.Order order)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.OrderUpdated);

        _viewModel = base.Ok(new GenericResponse<PutOrderResponse>(true, new PutOrderResponse(order), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.Order>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutOrderResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}