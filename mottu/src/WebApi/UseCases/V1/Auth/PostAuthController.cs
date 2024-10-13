using Application.UseCases.V1.Auth.PostAuth.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Auth.Request;
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

namespace WebApi.UseCases.V1.Auth;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/login", Name = "login")]
[ApiController]
[AllowAnonymous]
public class PostAuthController : CustomControllerBaseExtension, IOutputPortWithForbid<JwtResponse>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostAuthController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<JwtResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(GenericResponse<JwtResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<JwtResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Realizar login")]
    public async Task<IActionResult> PostAuth(
        [FromServices] IPostAuthUseCase useCase,
        [FromBody][Required] PostAuthRequest postAuthRequest)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(postAuthRequest);

        return _viewModel!;
    }

    void IOutputPortWithForbid<JwtResponse>.Ok(
        JwtResponse jwtResponse)
        => _viewModel = base.Ok(new GenericResponse<JwtResponse>(true, jwtResponse, null, NotificationTypeEnum.Success));

    void IOutputPortWithForbid<JwtResponse>.Forbid()
        => _viewModel = base.Unauthorized(new GenericResponse<JwtResponse>(true, null, _notificationHelper.Messages[SystemConst.Forbid]?.ToList(), NotificationTypeEnum.Warning));

    void IOutputPortWithForbid<JwtResponse>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<JwtResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}