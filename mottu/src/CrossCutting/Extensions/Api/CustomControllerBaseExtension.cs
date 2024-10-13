using CrossCutting.Common.Dtos.Response;
using CrossCutting.Enums;
using Domain.Common.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Extensions.Api;

public class CustomControllerBaseExtension : ControllerBase
{
    private bool _useCustomResponse;

    public CustomControllerBaseExtension(
       IFeatureManager featureManager)
    {
        _useCustomResponse =
            featureManager.IsEnabledAsync(nameof(CustomFeatureEnum.CustomResponse))
                          .ConfigureAwait(false)
                          .GetAwaiter()
                          .GetResult();
    }

    public override CreatedResult Created(
        string uri,
        object value)
    {
        if (_useCustomResponse)
            return base.Created(uri, value);

        var resultProperty = value?.GetType().GetProperty("Resultado");

        if (resultProperty == null)
            resultProperty = value?.GetType().GetProperty("ListaResultado");

        var result = resultProperty?.GetValue(value);

        return base.Created(uri, result);
    }

    public override OkObjectResult Ok(
        object value)
    {
        if (_useCustomResponse)
            return base.Ok(value);

        var resultProperty = value?.GetType().GetProperty("ListaNotificacao");

        var listNotification = resultProperty?.GetValue(value) as List<GenericNotificationResponse>;

        if (listNotification.Any())
        {
            var firstMessage = listNotification?.FirstOrDefault()?.Mensagem;

            return base.Ok(new { Mensagem = firstMessage ?? MessageConst.MessageEmpty });
        }
        else
        {
            resultProperty = value?.GetType().GetProperty("Resultado");

            if (resultProperty == null)
                resultProperty = value?.GetType().GetProperty("ListaResultado");

            var result = resultProperty?.GetValue(value);

            return base.Ok(result);
        }
    }

    public override BadRequestObjectResult BadRequest(
        object error)
    {
        if (_useCustomResponse)
            return base.BadRequest(error);

        var resultProperty = error?.GetType().GetProperty("ListaNotificacao");

        var listNotification = resultProperty?.GetValue(error) as List<GenericNotificationResponse>;

        var firstMessage = listNotification?.FirstOrDefault()?.Mensagem;

        return base.BadRequest(new { Mensagem = firstMessage ?? MessageConst.MessageEmpty });
    }

    public override NotFoundObjectResult NotFound(
        object value)
    {
        if (_useCustomResponse)
            return base.NotFound(value);

        var resultProperty = value?.GetType().GetProperty("ListaNotificacao");

        var listNotification = resultProperty?.GetValue(value) as List<GenericNotificationResponse>;

        var firstMessage = listNotification?.FirstOrDefault()?.Mensagem;

        return base.NotFound(new { Mensagem = firstMessage ?? MessageConst.MessageEmpty });
    }

    public override ConflictObjectResult Conflict(
        object value)
    {
        if (_useCustomResponse)
            return base.Conflict(value);

        var resultProperty = value?.GetType().GetProperty("ListaNotificacao");

        var listNotification = resultProperty?.GetValue(value) as List<GenericNotificationResponse>;

        var firstMessage = listNotification?.FirstOrDefault()?.Mensagem;

        return base.Conflict(new { Mensagem = firstMessage ?? MessageConst.MessageEmpty });
    }
}
