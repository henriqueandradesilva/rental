using Application.UseCases.V1.Driver.GetDriverById.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Driver.Response;
using CrossCutting.Enums;
using CrossCutting.Extensions.Api;
using CrossCutting.Extensions.UseCases;
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

namespace WebApi.UseCases.V1.Driver;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/entregadores", Name = "entregadores")]
[ApiController]
public class GetDriverByIdController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.Driver>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetDriverByIdController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<GetDriverResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<GetDriverResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<GetDriverResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Get))]
    [SwaggerOperation(Summary = "Consultar um entregador por id")]
    public async Task<IActionResult> GetDriver(
        [FromServices] IGetDriverByIdUseCase useCase,
        [FromRoute][Required] long id)
    {
        if (!ValidateUserExtension.IsOwnerOrAdminByDriverId(id, User))
        {
            _viewModel = base.Forbid();

            return _viewModel!;
        }

        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.Driver>.Ok(
        Domain.Entities.Driver driver)
        => _viewModel = base.Ok(new GenericResponse<GetDriverResponse>(true, new GetDriverResponse().GetDriver(driver), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.Driver>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<GetDriverResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.Driver>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<GetDriverResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}