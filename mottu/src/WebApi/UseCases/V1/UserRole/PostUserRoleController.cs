using Application.UseCases.V1.UserRole.PostUserRole.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.UserRole.Request;
using CrossCutting.Dtos.UserRole.Response;
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

namespace WebApi.UseCases.V1.UserRole;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tiposPerfil", Name = "tiposPerfil")]
[ApiController]
public class PostUserRoleController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.UserRole>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostUserRoleController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostUserRoleResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostUserRoleResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar um novo tipo de perfil de usuário")]
    public async Task<IActionResult> PostUserRole(
        [FromServices] IPostUserRoleUseCase useCase,
        [FromBody][Required] PostUserRoleRequest postUserRoleRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.UserRole userRole =
            new Domain.Entities.UserRole(
            0,
            postUserRoleRequest.Descricao);

        await useCase.Execute(userRole);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.UserRole>.Ok(
        Domain.Entities.UserRole userRole)
    {
        var uri = $"/tiposPerfil/{userRole.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.UserRoleCreated);

        var response =
            new GenericResponse<PostUserRoleResponse>(true, new PostUserRoleResponse(userRole), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.UserRole>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostUserRoleResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}