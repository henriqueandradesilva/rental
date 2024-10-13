using Application.UseCases.V1.User.PutUserSetActive.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.User.Request;
using CrossCutting.Dtos.User.Response;
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

namespace WebApi.UseCases.V1.User;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/usuarios", Name = "usuarios")]
[ApiController]
public class PutUserSetActiveController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.User>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutUserSetActiveController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}/ativo")]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutUserResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutUserResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Modificar campo ativo de um usu�rio por id")]
    public async Task<IActionResult> PutUserSetActive(
        [FromServices] IPutUserSetActiveUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutUserSetActiveRequest putUserSetActiveRequest)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(id, putUserSetActiveRequest.Ativo);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.User>.Ok(
        Domain.Entities.User user)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.UserUpdated);

        _viewModel = base.Ok(new GenericResponse<PutUserResponse>(true, new PutUserResponse(user), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.User>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutUserResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}