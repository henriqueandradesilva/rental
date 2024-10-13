using Application.UseCases.V1.OrderAccepted.GetOrderAcceptedById.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.OrderAccepted.Response;
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

namespace WebApi.UseCases.V1.OrderAccepted;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pedidosAceitos", Name = "pedidosAceitos")]
[ApiController]
public class GetOrderAcceptedByIdController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.OrderAccepted>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetOrderAcceptedByIdController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<GetOrderAcceptedResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<GetOrderAcceptedResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<GetOrderAcceptedResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Get))]
    [SwaggerOperation(Summary = "Consultar um pedido aceito por id")]
    public async Task<IActionResult> GetOrderAccepted(
        [FromServices] IGetOrderAcceptedByIdUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.OrderAccepted>.Ok(
        Domain.Entities.OrderAccepted orderAccepted)
        => _viewModel = base.Ok(new GenericResponse<GetOrderAcceptedResponse>(true, new GetOrderAcceptedResponse().GetOrderAccepted(orderAccepted), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.OrderAccepted>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<GetOrderAcceptedResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.OrderAccepted>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<GetOrderAcceptedResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}