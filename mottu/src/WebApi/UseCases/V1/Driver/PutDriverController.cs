using Application.UseCases.V1.Driver.PutDriver.Interfaces;
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
public class PutDriverController : CustomControllerBaseExtension, IOutputPort<Domain.Entities.Driver>
{
    private IActionResult _viewModel;
    private readonly NotificationHelper _notificationHelper;

    public PutDriverController(
        IFeatureManager featureManager,
        NotificationHelper notificationHelper) : base(featureManager)
    {
        _notificationHelper = notificationHelper;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{SystemConst.Admin},{SystemConst.Driver}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<PutDriverResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<PutDriverResponse>))]
    [ApiConventionMethod(typeof(CustomApiConvention), nameof(CustomApiConvention.Put))]
    [SwaggerOperation(Summary = "Alterar um entregador por id")]
    public async Task<IActionResult> PutDriver(
        [FromServices] IPutDriverUseCase useCase,
        [FromRoute][Required] long id,
        [FromBody][Required] PutDriverRequest putDriverRequest)
    {
        if (!ValidateUserExtension.IsOwnerOrAdminByDriverId(id, User))
        {
            _viewModel = base.Forbid();

            return _viewModel!;
        }

        if (!ValidateUserExtension.CheckCnhType(putDriverRequest.Tipo_Cnh))
        {
            var listNotification = new List<string>();
            listNotification.Add(MessageConst.DriverCnhTypeInvalid);

            _viewModel = base.BadRequest(new GenericResponse<PutDriverResponse>(false, null, listNotification, NotificationTypeEnum.Warning));

            return _viewModel!;
        }

        useCase.SetOutputPort(this);

        Domain.Entities.Driver driver =
            new Domain.Entities.Driver(
            id,
            putDriverRequest.Identificador,
            putDriverRequest.Cnpj,
            putDriverRequest.Nome,
            putDriverRequest.Data_Nascimento,
            putDriverRequest.Numero_Cnh,
            putDriverRequest.Tipo_Cnh);

        await useCase.Execute(driver, putDriverRequest.Imagem_Cnh);

        return _viewModel!;
    }

    void IOutputPort<Domain.Entities.Driver>.Ok(
        Domain.Entities.Driver driver)
    {
        var listNotification = new List<string>();
        listNotification.Add(MessageConst.DriverUpdated);

        _viewModel = base.Ok(new GenericResponse<PutDriverResponse>(true, new PutDriverResponse(driver), listNotification, NotificationTypeEnum.Success));
    }

    void IOutputPort<Domain.Entities.Driver>.Error()
        => _viewModel = base.BadRequest(new GenericResponse<PutDriverResponse>(false, null, _notificationHelper.Messages[SystemConst.Error]?.ToList(), NotificationTypeEnum.Error));
}