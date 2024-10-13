using Application.UseCases.V1.ModelVehicle.GetModelVehicleById.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.ModelVehicle.Response;
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

namespace WebApi.UseCases.V1.ModelVehicle;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/modelosVeiculo", Name = "modelosVeiculo")]
[ApiController]
public class GetModelVehicleByIdController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.ModelVehicle>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetModelVehicleByIdController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<GetModelVehicleResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<GetModelVehicleResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<GetModelVehicleResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Get))]
    [SwaggerOperation(Summary = "Consultar um modelo de veículo por id")]
    public async Task<IActionResult> GetModelVehicle(
        [FromServices] IGetModelVehicleByIdUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.ModelVehicle>.Ok(
        Domain.Entities.ModelVehicle modelVehicle)
        => _viewModel = base.Ok(new GenericResponse<GetModelVehicleResponse>(true, new GetModelVehicleResponse().GetModelVehicle(modelVehicle), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.ModelVehicle>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<GetModelVehicleResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.ModelVehicle>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<GetModelVehicleResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}