using Application.UseCases.V1.Motorcycle.DeleteMotorcycle.Interfaces;
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
public class DeleteMotorcycleController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.Motorcycle>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public DeleteMotorcycleController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<DeleteMotorcycleResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<DeleteMotorcycleResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<DeleteMotorcycleResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Delete))]
    [SwaggerOperation(Summary = "Remover uma moto por id")]
    public async Task<IActionResult> DeleteMotorcycle(
        [FromServices] IDeleteMotorcycleUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.Motorcycle>.Ok(
        Domain.Entities.Motorcycle motorcycle)
        => _viewModel = base.Ok(new GenericResponse<DeleteMotorcycleResponse>(true, new DeleteMotorcycleResponse(motorcycle), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.Motorcycle>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<DeleteMotorcycleResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.Motorcycle>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<DeleteMotorcycleResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}