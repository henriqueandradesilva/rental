using Application.UseCases.V1.User.PostUser.Interfaces;
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
public class PostUserController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.User>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostUserController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [Authorize(Roles = SystemConst.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostUserResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostUserResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar um usuário")]
    public async Task<IActionResult> PostUser(
        [FromServices] IPostUserUseCase useCase,
        [FromBody][Required] PostUserRequest postUserRequest)
    {
        useCase.SetOutputPort(this);

        Domain.Entities.User user =
            new Domain.Entities.User(
            0,
            postUserRequest.TipoPerfilId,
            postUserRequest.Nome,
            postUserRequest.Email,
            postUserRequest.Senha,
            postUserRequest.Ativo);

        await useCase.Execute(user);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.User>.Ok(
        Domain.Entities.User user)
    {
        var uri = $"/usuarios/{user.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.UserCreated);

        var response =
            new GenericResponse<PostUserResponse>(true, new PostUserResponse(user), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.User>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostUserResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}