using Application.UseCases.V1.Order.GetListAllOrder.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Order.Response;
using CrossCutting.Enums;
using CrossCutting.Extensions.Api;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Order;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pedidos", Name = "pedidos")]
[ApiController]
public class GetListAllOrderController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<Domain.Entities.Order>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllOrderController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("listar")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListOrderResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListOrderResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListOrderResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Listar todos os pedidos")]
    public async Task<IActionResult> GetListOrder(
        [FromServices] IGetListAllOrderUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<Domain.Entities.Order>>.Ok(
        List<Domain.Entities.Order> listOrder)
        => _viewModel = base.Ok(new GenericResponse<List<GetListOrderResponse>>(true, new GetListOrderResponse().GetListOrder(listOrder), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<Domain.Entities.Order>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListOrderResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<Domain.Entities.Order>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListOrderResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}