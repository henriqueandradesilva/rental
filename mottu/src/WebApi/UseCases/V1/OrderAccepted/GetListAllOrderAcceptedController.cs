using Application.UseCases.V1.OrderAccepted.GetListAllOrderAccepted.Interfaces;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.OrderAccepted;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/pedidosAceitos", Name = "pedidosAceitos")]
[ApiController]
public class GetListAllOrderAcceptedController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<Domain.Entities.OrderAccepted>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllOrderAcceptedController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("listar")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListOrderAcceptedResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListOrderAcceptedResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListOrderAcceptedResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Listar todos os pedidos aceitos")]
    public async Task<IActionResult> GetListOrderAccepted(
        [FromServices] IGetListAllOrderAcceptedUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<Domain.Entities.OrderAccepted>>.Ok(
        List<Domain.Entities.OrderAccepted> listOrderAccepted)
        => _viewModel = base.Ok(new GenericResponse<List<GetListOrderAcceptedResponse>>(true, new GetListOrderAcceptedResponse().GetListOrderAccepted(listOrderAccepted), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<Domain.Entities.OrderAccepted>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListOrderAcceptedResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<Domain.Entities.OrderAccepted>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListOrderAcceptedResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}