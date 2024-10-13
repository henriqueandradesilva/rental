using Application.UseCases.V1.Order.PostOrder.Interfaces;
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
public class PostOrderController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Order>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostOrderController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostOrderResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostOrderResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar um novo pedido")]
    public async Task<IActionResult> PostOrder(
        [FromServices] IPostOrderUseCase useCase,
        [FromBody][Required] PostOrderRequest postOrderRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.Order order =
            new Domain.Entities.Order(
            0,
            SystemConst.OrderStatusAvailableDefault,
            postOrderRequest.Descricao,
            postOrderRequest.Valor,
            postOrderRequest.Data);

        await useCase.Execute(order);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Order>.Ok(
        Domain.Entities.Order order)
    {
        var uri = $"/pedidos/{order.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.OrderCreated);

        var response =
            new GenericResponse<PostOrderResponse>(true, new PostOrderResponse(order), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.Order>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostOrderResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}