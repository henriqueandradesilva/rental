using Application.UseCases.V1.OrderStatus.PostOrderStatus.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.OrderStatus.Request;
using CrossCutting.Dtos.OrderStatus.Response;
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

namespace WebApi.UseCases.V1.OrderStatus;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/statusPedido", Name = "statusPedido")]
[ApiController]
public class PostOrderStatusController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.OrderStatus>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostOrderStatusController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostOrderStatusResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostOrderStatusResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar um novo tipo de status de pedido")]
    public async Task<IActionResult> PostOrderStatus(
        [FromServices] IPostOrderStatusUseCase useCase,
        [FromBody][Required] PostOrderStatusRequest postOrderStatusRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.OrderStatus orderStatus =
            new Domain.Entities.OrderStatus(
            0,
            postOrderStatusRequest.Descricao);

        await useCase.Execute(orderStatus);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.OrderStatus>.Ok(
        Domain.Entities.OrderStatus orderStatus)
    {
        var uri = $"/statusPedido/{orderStatus.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.OrderStatusCreated);

        var response =
            new GenericResponse<PostOrderStatusResponse>(true, new PostOrderStatusResponse(orderStatus), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.OrderStatus>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostOrderStatusResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}