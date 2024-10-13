using Application.UseCases.V1.OrderAccepted.PutOrderAccepted.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.OrderAccepted.Request;
using CrossCutting.Dtos.OrderAccepted.Response;
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

namespace WebApi.UseCases.V1.OrderAccepted;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pedidosAceitos", Name = "pedidosAceitos")]
[ApiController]
public class PutOrderAcceptedController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.OrderAccepted>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutOrderAcceptedController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutOrderAcceptedResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutOrderAcceptedResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar um pedido aceito por id")]
    public async Task<IActionResult> PutOrderAccepted(
        [FromServices] IPutOrderAcceptedUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutOrderAcceptedRequest putOrderAcceptedRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.OrderAccepted orderAccepted =
            new Domain.Entities.OrderAccepted(
            id,
            putOrderAcceptedRequest.EntregadorId,
            putOrderAcceptedRequest.PedidoId,
            putOrderAcceptedRequest.Data);

        await useCase.Execute(orderAccepted);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.OrderAccepted>.Ok(
        Domain.Entities.OrderAccepted orderAccepted)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.OrderAccepted);

        _viewModel = base.Ok(new GenericResponse<PutOrderAcceptedResponse>(true, new PutOrderAcceptedResponse(orderAccepted), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.OrderAccepted>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutOrderAcceptedResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}