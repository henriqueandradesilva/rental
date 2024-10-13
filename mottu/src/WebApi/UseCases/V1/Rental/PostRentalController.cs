using Application.UseCases.V1.Rental.PostRental.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Rental.Request;
using CrossCutting.Dtos.Rental.Response;
using CrossCutting.Enums;
using CrossCutting.Extensions.Api;
using CrossCutting.Extensions.UseCases;
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
public class PostRentalController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Rental>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostRentalController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostRentalResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostRentalResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Alugar uma moto")]
    public async Task<IActionResult> PostRental(
        [FromServices] IPostRentalUseCase useCase,
        [FromBody][Required] PostRentalRequest postRentalRequest)
    {
        if (!ValidateUserExtension.IsOwnerOrAdminByDriverId(postRentalRequest.Entregador_Id, User))
        {
            _viewModel = base.Forbid();

            return _viewModel!;
        }

        useCase.SetOutputPort(this);

        Domain.Entities.Rental rental =
            new Domain.Entities.Rental(
            0,
            postRentalRequest.Moto_Id,
            postRentalRequest.Entregador_Id,
            postRentalRequest.Plano_Id,
            postRentalRequest.Data_Inicio,
            Domain.Common.Enums.RentalStatusEnum.Pending);

        await useCase.Execute(rental);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Rental>.Ok(
        Domain.Entities.Rental rental)
    {
        var uri = $"/locacao/{rental.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.RentalCreated);

        var response =
            new GenericResponse<PostRentalResponse>(true, new PostRentalResponse(rental), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.Rental>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostRentalResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}