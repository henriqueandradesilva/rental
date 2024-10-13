using Application.UseCases.V1.User.GetUserById.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.User.Response;
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

namespace WebApi.UseCases.V1.User;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/usuarios", Name = "usuarios")]
[ApiController]
public class GetUserByIdController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.User>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public GetUserByIdController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<GetUserResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<GetUserResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<GetUserResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Get))]
    [SwaggerOperation(Summary = "Consultar um usuário por id")]
    public async Task<IActionResult> GetUser(
        [FromServices] IGetUserByIdUseCase useCase,
        [FromRoute][Required] long id)
    {
        if (!ValidateUserExtension.IsOwnerOrAdminByUserId(id, User))
        {
            _viewModel = base.Forbid();

            return _viewModel!;
        }

        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.User>.Ok(
        Domain.Entities.User user)
        => _viewModel = base.Ok(new GenericResponse<GetUserResponse>(true, new GetUserResponse().GetUser(user), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.User>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<GetUserResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.User>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<GetUserResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}