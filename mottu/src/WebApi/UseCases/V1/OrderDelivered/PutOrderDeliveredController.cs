using Application.UseCases.V1.OrderDelivered.PutOrderDelivered.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.OrderDelivered.Request;
using CrossCutting.Dtos.OrderDelivered.Response;
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

namespace WebApi.UseCases.V1.OrderDelivered;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pedidosEntregues", Name = "pedidosEntregues")]
[ApiController]
public class PutOrderDeliveredController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.OrderDelivered>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutOrderDeliveredController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutOrderDeliveredResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutOrderDeliveredResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar um pedido entregue por id")]
    public async Task<IActionResult> PutOrderDelivered(
        [FromServices] IPutOrderDeliveredUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutOrderDeliveredRequest putOrderDeliveredRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.OrderDelivered orderDelivered =
            new Domain.Entities.OrderDelivered(
            id,
            putOrderDeliveredRequest.EntregadorId,
            putOrderDeliveredRequest.PedidoId,
            putOrderDeliveredRequest.Data);

        await useCase.Execute(orderDelivered);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.OrderDelivered>.Ok(
        Domain.Entities.OrderDelivered orderDelivered)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.OrderDelivered);

        _viewModel = base.Ok(new GenericResponse<PutOrderDeliveredResponse>(true, new PutOrderDeliveredResponse(orderDelivered), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.OrderDelivered>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutOrderDeliveredResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}