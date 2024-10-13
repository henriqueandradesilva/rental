using Application.UseCases.V1.OrderDelivered.GetOrderDeliveredById.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.OrderDelivered.Response;
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

namespace WebApi.UseCases.V1.OrderDelivered;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pedidosEntregues", Name = "pedidosEntregues")]
[ApiController]
public class GetOrderDeliveredByIdController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.OrderDelivered>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetOrderDeliveredByIdController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<GetOrderDeliveredResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<GetOrderDeliveredResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<GetOrderDeliveredResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Get))]
    [SwaggerOperation(Summary = "Consultar um pedido entregue por id")]
    public async Task<IActionResult> GetOrderDelivered(
        [FromServices] IGetOrderDeliveredByIdUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.OrderDelivered>.Ok(
        Domain.Entities.OrderDelivered orderDelivered)
        => _viewModel = base.Ok(new GenericResponse<GetOrderDeliveredResponse>(true, new GetOrderDeliveredResponse().GetOrderDelivered(orderDelivered), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.OrderDelivered>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<GetOrderDeliveredResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.OrderDelivered>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<GetOrderDeliveredResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}