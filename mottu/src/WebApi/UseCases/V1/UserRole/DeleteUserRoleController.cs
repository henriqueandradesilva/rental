using Application.UseCases.V1.UserRole.DeleteUserRole.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.UserRole.Response;
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

namespace WebApi.UseCases.V1.UserRole;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tiposPerfil", Name = "tiposPerfil")]
[ApiController]
public class DeleteUserRoleController : CustomControllerBaseExtension, IOutputPortWithNotFound<Domain.Entities.UserRole>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public DeleteUserRoleController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<DeleteUserRoleResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<DeleteUserRoleResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<DeleteUserRoleResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Delete))]
    [SwaggerOperation(Summary = "Remover um tipo de perfil de usu�rio por id")]
    public async Task<IActionResult> DeleteUserRole(
        [FromServices] IDeleteUserRoleUseCase useCase,
        [FromRoute][Required] long id)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id);

        return _viewModel!;
    }

    void IOutputPortWithNotFound<Domain.Entities.UserRole>.Ok(
        Domain.Entities.UserRole userRole)
        => _viewModel = base.Ok(new GenericResponse<DeleteUserRoleResponse>(true, new DeleteUserRoleResponse(userRole), null, NotificationTypeEnum.Success));

    void IOutputPortWithNotFound<Domain.Entities.UserRole>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<DeleteUserRoleResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));

    void IOutputPortWithNotFound<Domain.Entities.UserRole>.NotFound()
        => _viewModel = base.NotFound(new GenericResponse<DeleteUserRoleResponse>(true, null, _notificationHelper.Messages[SystemConst.NotFound]?.ToList(), NotificationTypeEnum.Warning));
}