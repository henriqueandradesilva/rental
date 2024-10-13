using Application.UseCases.V1.Motorcycle.GetListAllMotorcycle.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Motorcycle.Response;
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

namespace WebApi.UseCases.V1.Motorcycle;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/motos", Name = "motos")]
[ApiController]
public class GetListAllMotorcycleController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<Domain.Entities.Motorcycle>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllMotorcycleController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("listar")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListMotorcycleResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListMotorcycleResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListMotorcycleResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Listar todas as motos")]
    public async Task<IActionResult> GetListMotorcycle(
        [FromServices] IGetListAllMotorcycleUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<Domain.Entities.Motorcycle>>.Ok(
        List<Domain.Entities.Motorcycle> listMotorcycle)
        => _viewModel = base.Ok(new GenericResponse<List<GetListMotorcycleResponse>>(true, new GetListMotorcycleResponse().GetListMotorcycle(listMotorcycle), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<Domain.Entities.Motorcycle>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListMotorcycleResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<Domain.Entities.Motorcycle>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListMotorcycleResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}