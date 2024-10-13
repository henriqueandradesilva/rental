using Application.UseCases.V1.Driver.GetListDriverByCnh.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Driver.Response;
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.UseCases.V1.Driver;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/entregadores", Name = "entregadores")]
[ApiController]
public class GetListDriverByCnhController : CustomControllerBaseExtension, IOutputPortWithNotFound<List<Domain.Entities.Driver>>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetListDriverByCnhController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("{cnh}/cnh")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<List<GetListDriverResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<List<GetListDriverResponse>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<List<GetListDriverResponse>>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.List))]
    [SwaggerOperation(Summary = "Consultar entregadores por CNH")]
    public async Task<IActionResult> GetDriver(
        [FromServices] IGetListDriverByCnhUseCase useCase,
        [FromRoute][Required] string cnh)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(cnh);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<List<Domain.Entities.Driver>>.Ok(
    List<Domain.Entities.Driver> listDriver)
    => _viewModel = base.Ok(new GenericResponse<List<GetListDriverResponse>>(true, new GetListDriverResponse().GetListDriver(listDriver), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<List<Domain.Entities.Driver>>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<List<GetListDriverResponse>>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<List<Domain.Entities.Driver>>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<List<GetListDriverResponse>>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}