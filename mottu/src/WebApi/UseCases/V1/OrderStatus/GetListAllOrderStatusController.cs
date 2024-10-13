using Application.UseCases.V1.OrderStatus.GetListAllOrderStatus.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.OrderStatus.Response;
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

namespace WebApi.UseCases.V1.OrderStatus;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/statusPedido", Name = "statusPedido")]
[ApiController]
public class GetListAllOrderStatusController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<Domain.Entities.OrderStatus>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllOrderStatusController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("listar")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListOrderStatusResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListOrderStatusResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListOrderStatusResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Listar todos os tipos de status de pedido")]
    public async Task<IActionResult> GetListOrderStatus(
        [FromServices] IGetListAllOrderStatusUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<Domain.Entities.OrderStatus>>.Ok(
        List<Domain.Entities.OrderStatus> listOrderStatus)
        => _viewModel = base.Ok(new GenericResponse<List<GetListOrderStatusResponse>>(true, new GetListOrderStatusResponse().GetListOrderStatus(listOrderStatus), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<Domain.Entities.OrderStatus>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListOrderStatusResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<Domain.Entities.OrderStatus>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListOrderStatusResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}