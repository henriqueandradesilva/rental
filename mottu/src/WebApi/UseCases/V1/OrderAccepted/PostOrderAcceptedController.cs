using Application.UseCases.V1.OrderAccepted.PostOrderAccepted.Interfaces;
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
public class PostOrderAcceptedController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.OrderAccepted>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostOrderAcceptedController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostOrderAcceptedResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostOrderAcceptedResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar um novo pedido aceito")]
    public async Task<IActionResult> PostOrderAccepted(
        [FromServices] IPostOrderAcceptedUseCase useCase,
        [FromBody][Required] PostOrderAcceptedRequest postOrderAcceptedRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.OrderAccepted orderAccepted =
            new Domain.Entities.OrderAccepted(
            0,
            postOrderAcceptedRequest.EntregadorId,
            postOrderAcceptedRequest.PedidoId,
            postOrderAcceptedRequest.Data);

        await useCase.Execute(orderAccepted);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.OrderAccepted>.Ok(
        Domain.Entities.OrderAccepted orderAccepted)
    {
        var uri = $"/pedidosAceitos/{orderAccepted.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.OrderAccepted);

        var response =
            new GenericResponse<PostOrderAcceptedResponse>(true, new PostOrderAcceptedResponse(orderAccepted), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.OrderAccepted>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostOrderAcceptedResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}