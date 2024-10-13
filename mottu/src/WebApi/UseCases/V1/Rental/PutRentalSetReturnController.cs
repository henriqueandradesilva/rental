using Application.UseCases.V1.Rental.PutRentalSetReturn.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Rental.Request;
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
public class PutRentalSetReturnController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Rental>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutRentalSetReturnController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}/devolucao")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutRentalResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutRentalResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Informar data de devolução e calcular valor")]
    public async Task<IActionResult> PutRentalSetReturn(
        [FromServices] IPutRentalSetReturnUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutRentalSetReturnRequest putRentalSetReturnRequest)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id, putRentalSetReturnRequest.Data_Devolucao, User);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Rental>.Ok(
        Domain.Entities.Rental rental)
        => _viewModel = base.Ok(new GenericResponse<GetRentalResponse>(true, new GetRentalResponse().GetRental(rental), null, NotificationTypeEnum.Success));

    void IOutputPort<Domain.Entities.Rental>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<GetRentalResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}