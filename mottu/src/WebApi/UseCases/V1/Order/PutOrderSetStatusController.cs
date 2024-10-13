using Application.UseCases.V1.Order.PutOrderSetStatus.Interfaces;
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
public class PutOrderSetStatusController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Order>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutOrderSetStatusController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutOrderResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutOrderResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Modificar o status de um pedido")]
    public async Task<IActionResult> PutOrderSetStatus(
        [FromServices] IPutOrderSetStatusUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutOrderSetStatusRequest putOrderSetStatusRequest)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id, putOrderSetStatusRequest.StatusId);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Order>.Ok(
        Domain.Entities.Order Order)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.OrderUpdated);

        _viewModel = base.Ok(new GenericResponse<PutOrderResponse>(true, new PutOrderResponse(Order), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.Order>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutOrderResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}