using Application.UseCases.V1.Auth.PostAuth.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Dtos.Auth.Request;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Auth.PostAuth;

public class PostAuthValidationUseCase : IPostAuthUseCase
{
    private IOutputPortWithForbid<JwtResponse> _outputPort;
    private readonly IPostAuthUseCase _useCase;
    private readonly NotificationHelper _notificationHelper;

    public PostAuthValidationUseCase(
        IPostAuthUseCase useCase,
        NotificationHelper notificationHelper)
    {
        _useCase = useCase;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        PostAuthRequest postAuthRequest)
    {
        try
        {
            if (string.IsNullOrEmpty(postAuthRequest.Email))
            {
                _notificationHelper.Add(SystemConst.Error, SystemConst.AuthEmailRequired);

                _outputPort.Error();

                return;
            }

            if (string.IsNullOrEmpty(postAuthRequest.Senha))
            {
                _notificationHelper.Add(SystemConst.Error, SystemConst.AuthPasswordRequired);

                _outputPort.Error();

                return;
            }

            await _useCase.Execute(postAuthRequest);
        }
        catch (Exception ex)
        {
            _notificationHelper.Add(SystemConst.Error, ex.Message);

            if (_notificationHelper.HasMessage)
                _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPortWithForbid<JwtResponse> outputPort)
    {
        _outputPort = outputPort;
        _useCase.SetOutputPort(outputPort);
    }
}