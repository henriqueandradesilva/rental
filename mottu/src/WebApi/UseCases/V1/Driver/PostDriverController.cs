using Application.UseCases.V1.Driver.PostDriver.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Conventations;
using CrossCutting.Dtos.Driver.Request;
using CrossCutting.Dtos.Driver.Response;
using CrossCutting.Enums;
using CrossCutting.Extensions.Api;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
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
public class PostDriverController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Driver>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PostDriverController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<PostDriverResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PostDriverResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Post))]
    [SwaggerOperation(Summary = "Cadastrar um novo entregador")]
    public async Task<IActionResult> PostDriver(
        [FromServices] IPostDriverUseCase useCase,
        [FromBody][Required] PostDriverRequest postDriverRequest)
    {
        if (!ValidateUserExtension.CheckCnhType(postDriverRequest.Tipo_Cnh))
        {
            var listNotification = new List<string>();
            listNotification.Add(MessageConst.DriverCnhTypeInvalid);

            _viewModel = base.BadRequest(new GenericResponse<PostDriverResponse>(false, null, listNotification, NotificationTypeEnum.Warning));

            return _viewModel!;
        }

        useCase.SetOutputPort(this);

        Domain.Entities.Driver driver =
            new Domain.Entities.Driver(
            0,
            postDriverRequest.Identificador,
            postDriverRequest.Cnpj,
            postDriverRequest.Nome,
            postDriverRequest.Data_Nascimento,
            postDriverRequest.Numero_Cnh,
            postDriverRequest.Tipo_Cnh);

        await useCase.Execute(driver, postDriverRequest.Imagem_Cnh);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Driver>.Ok(
        Domain.Entities.Driver driver)
    {
        var uri = $"/entregadores/{driver.Id}";

        var listNotification = new List<string>();
        listNotification.Add(MessageConst.DriverCreated);

        var response =
            new GenericResponse<PostDriverResponse>(true, new PostDriverResponse(driver), listNotification, NotificationTypeEnum.Success);

        _viewModel = base.Created(uri, response);
    }

    void IOutputPort<Domain.Entities.Driver>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PostDriverResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}