using Application.UseCases.V1.Rental.PutRental.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Rental.Request;
using CrossCutting.Dtos.Rental.Response;
using CrossCutting.Enums;
using CrossCutting.Extensions.Api;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Rental;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/locacao", Name = "locacao")]
[ApiController]
public class PutRentalController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Rental>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutRentalController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutRentalResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutRentalResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar uma locação por id")]
    public async Task<IActionResult> PutRental(
        [FromServices] IPutRentalUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutRentalRequest putRentalRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.Rental rental =
            new Domain.Entities.Rental(
            id,
            putRentalRequest.Moto_Id,
            putRentalRequest.Entregador_Id,
            putRentalRequest.Plano_Id,
            putRentalRequest.Data_Inicio,
            Domain.Common.Enums.RentalStatusEnum.Pending);

        await useCase.Execute(rental);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Rental>.Ok(
        Domain.Entities.Rental rental)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.RentalUpdated);

        _viewModel = base.Ok(new GenericResponse<PutRentalResponse>(true, new PutRentalResponse(rental), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.Rental>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutRentalResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}