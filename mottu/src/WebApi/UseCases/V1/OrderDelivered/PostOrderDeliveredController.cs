using Application.UseCases.V1.OrderDelivered.PostOrderDelivered.Interfaces;
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
public class PostOrderDeliveredController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.OrderDelivered>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostOrderDeliveredController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostOrderDeliveredResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostOrderDeliveredResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar um novo pedido entregue")]
    public async Task<IActionResult> PostOrderDelivered(
        [FromServices] IPostOrderDeliveredUseCase useCase,
        [FromBody][Required] PostOrderDeliveredRequest postOrderDeliveredRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.OrderDelivered orderDelivered =
            new Domain.Entities.OrderDelivered(
            0,
            postOrderDeliveredRequest.EntregadorId,
            postOrderDeliveredRequest.PedidoId,
            postOrderDeliveredRequest.Data);

        await useCase.Execute(orderDelivered);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.OrderDelivered>.Ok(
        Domain.Entities.OrderDelivered orderDelivered)
    {
        var uri = $"/pedidosAceitos/{orderDelivered.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.OrderDelivered);

        var response =
            new GenericResponse<PostOrderDeliveredResponse>(true, new PostOrderDeliveredResponse(orderDelivered), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.OrderDelivered>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostOrderDeliveredResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}