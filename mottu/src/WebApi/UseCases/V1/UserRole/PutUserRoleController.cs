using Application.UseCases.V1.UserRole.PutUserRole.Interfaces;
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
public class PutUserRoleController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.UserRole>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutUserRoleController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutUserRoleResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutUserRoleResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar um tipo de perfil de usuário por id")]
    public async Task<IActionResult> PutUserRole(
        [FromServices] IPutUserRoleUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutUserRoleRequest putUserRoleRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.UserRole userRole =
            new Domain.Entities.UserRole(
            id,
            putUserRoleRequest.Descricao);

        await useCase.Execute(userRole);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.UserRole>.Ok(
        Domain.Entities.UserRole userRole)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.UserRoleUpdated);

        _viewModel = base.Ok(new GenericResponse<PutUserRoleResponse>(true, new PutUserRoleResponse(userRole), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.UserRole>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutUserRoleResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}