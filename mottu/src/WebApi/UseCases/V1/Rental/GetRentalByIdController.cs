using Application.UseCases.V1.Rental.GetRentalById.Interfaces;
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
public class GetRentalByIdController : CustomControllerBaseExtension, IOutputPortWithForbid<Domain.Entities.Rental>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetRentalByIdController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<GetRentalResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<GetRentalResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<GetRentalResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Get))]
    [SwaggerOperation(Summary = "Consultar uma locação por id")]
    public async Task<IActionResult> GetRental(
        [FromServices] IGetRentalByIdUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id, User);

        return _viewModel!;
    }

    void IOutputPortWithForbid<Domain.Entities.Rental>.Ok(
        Domain.Entities.Rental rental)
        => _viewModel = base.Ok(new GenericResponse<GetRentalResponse>(true, new GetRentalResponse().GetRental(rental), null, NotificationTypeEnum.Success));

    void IOutputPortWithForbid<Domain.Entities.Rental>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<GetRentalResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithForbid<Domain.Entities.Rental>.Forbid()
        => _viewModel = base.NotFound(new GenericResponse<GetRentalResponse>(true, null, _notificationHelper.Messages[SystemConst.Forbid]?.ToList(), NotificationTypeEnum.Warning));
}