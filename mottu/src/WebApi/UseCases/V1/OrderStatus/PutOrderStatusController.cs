using Application.UseCases.V1.OrderStatus.PutOrderStatus.Interfaces;
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
public class PutOrderStatusController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.OrderStatus>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutOrderStatusController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutOrderStatusResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutOrderStatusResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar um tipo de status de pedido por id")]
    public async Task<IActionResult> PutOrderStatus(
        [FromServices] IPutOrderStatusUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutOrderStatusRequest putOrderStatusRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.OrderStatus orderStatus =
            new Domain.Entities.OrderStatus(
            id,
            putOrderStatusRequest.Descricao);

        await useCase.Execute(orderStatus);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.OrderStatus>.Ok(
        Domain.Entities.OrderStatus orderStatus)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.OrderStatusUpdated);

        _viewModel = base.Ok(new GenericResponse<PutOrderStatusResponse>(true, new PutOrderStatusResponse(orderStatus), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.OrderStatus>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutOrderStatusResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}