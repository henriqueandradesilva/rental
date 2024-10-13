using Application.UseCases.V1.ModelVehicle.GetListAllModelVehicle.Interfaces;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.ModelVehicle;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/modelosVeiculo", Name = "modelosVeiculo")]
[ApiController]
public class GetListAllModelVehicleController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<Domain.Entities.ModelVehicle>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListAllModelVehicleController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("listar")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListModelVehicleResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListModelVehicleResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListModelVehicleResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Listar todos os modelos de veículos")]
    public async Task<IActionResult> GetListModelVehicle(
        [FromServices] IGetListAllModelVehicleUseCase useCase)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute();

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<Domain.Entities.ModelVehicle>>.Ok(
        List<Domain.Entities.ModelVehicle> listModelVehicle)
        => _viewModel = base.Ok(new GenericResponse<List<GetListModelVehicleResponse>>(true, new GetListModelVehicleResponse().GetListModelVehicle(listModelVehicle), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<Domain.Entities.ModelVehicle>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListModelVehicleResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<Domain.Entities.ModelVehicle>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListModelVehicleResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}