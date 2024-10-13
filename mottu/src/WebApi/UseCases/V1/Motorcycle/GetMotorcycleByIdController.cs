using Application.UseCases.V1.Motorcycle.GetMotorcycleById.Interfaces;
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Motorcycle;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/motos", Name = "motos")]
[ApiController]
public class GetMotorcycleByIdController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.Motorcycle>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetMotorcycleByIdController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<GetMotorcycleResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<GetMotorcycleResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<GetMotorcycleResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Get))]
    [SwaggerOperation(Summary = "Consultar uma moto por id")]
    public async Task<IActionResult> GetMotorcycle(
        [FromServices] IGetMotorcycleByIdUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.Motorcycle>.Ok(
        Domain.Entities.Motorcycle motorcycle)
        => _viewModel = base.Ok(new GenericResponse<GetMotorcycleResponse>(true, new GetMotorcycleResponse().GetMotorcycle(motorcycle), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.Motorcycle>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<GetMotorcycleResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.Motorcycle>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<GetMotorcycleResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}