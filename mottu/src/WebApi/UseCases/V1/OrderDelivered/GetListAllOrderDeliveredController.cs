using Application.UseCases.V1.OrderDelivered.GetListAllOrderDelivered.Interfaces;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.OrderDelivered;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pedidosEntregues", Name = "pedidosEntregues")]
[ApiController]
public class GetListAllOrderDeliveredController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<Domain.Entities.OrderDelivered>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllOrderDeliveredController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("listar")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListOrderDeliveredResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListOrderDeliveredResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListOrderDeliveredResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Listar todos os pedidos entregues")]
    public async Task<IActionResult> GetListOrderDelivered(
        [FromServices] IGetListAllOrderDeliveredUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<Domain.Entities.OrderDelivered>>.Ok(
        List<Domain.Entities.OrderDelivered> listOrderDelivered)
        => _viewModel = base.Ok(new GenericResponse<List<GetListOrderDeliveredResponse>>(true, new GetListOrderDeliveredResponse().GetListOrderDelivered(listOrderDelivered), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<Domain.Entities.OrderDelivered>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListOrderDeliveredResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<Domain.Entities.OrderDelivered>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListOrderDeliveredResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}