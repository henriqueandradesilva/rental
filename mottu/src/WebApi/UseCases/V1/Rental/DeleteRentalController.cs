using Application.UseCases.V1.Rental.DeleteRental.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Rental.Response;
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

namespace WebApi.UseCases.V1.Rental;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/locacao", Name = "locacao")]
[ApiController]
public class DeleteRentalController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.Rental>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public DeleteRentalController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<DeleteRentalResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<DeleteRentalResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<DeleteRentalResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Delete))]
    [SwaggerOperation(Summary = "Remover uma locação por id")]
    public async Task<IActionResult> DeleteRental(
        [FromServices] IDeleteRentalUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.Rental>.Ok(
        Domain.Entities.Rental rental)
        => _viewModel = base.Ok(new GenericResponse<DeleteRentalResponse>(true, new DeleteRentalResponse(rental), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.Rental>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<DeleteRentalResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.Rental>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<DeleteRentalResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}