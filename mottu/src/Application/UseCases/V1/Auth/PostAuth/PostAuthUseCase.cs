using Application.Services;
using Application.UseCases.V1.Auth.PostAuth.Interfaces;
using CrossCutting.Common.Dtos.Response;
using CrossCutting.Const;
using CrossCutting.Dtos.Auth.Request;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using CrossCutting.Settings;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Auth.PostAuth;

public class PostAuthUseCase : IPostAuthUseCase
{
    private IOutputPortWithForbid<JwtResponse> _outputPort;
    private IUnitOfWork _unitOfWork;
    private IConfiguration _configuration;
    private IUserRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public PostAuthUseCase(
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IUserRepository repository,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        PostAuthRequest postAuthRequest)
    {
        Domain.Entities.User userExist = null;

        if (!string.IsNullOrEmpty(postAuthRequest.Email))
        {
            userExist =
                await _repository?.Where(c => c.Email.ToUpper() == postAuthRequest.Email.ToUpper())
                                 ?.Include(c => c.UserRole)
                                 ?.Include(c => c.Driver)
                                 ?.FirstOrDefaultAsync();
        }

        if (userExist == null || !Verify(postAuthRequest.Senha, userExist.Password))
        {
            _notificationHelper.Add(SystemConst.Forbid, SystemConst.AuthEmailOrPasswordInvalid);

            _outputPort.Forbid();

            return;
        }

        if (!userExist.IsActive)
        {
            _notificationHelper.Add(SystemConst.Error, SystemConst.AuthAccountIsNotActive);

            _outputPort.Error();

            return;
        }

        _repository.Update(userExist);

        await _unitOfWork.Save()
                         .ConfigureAwait(false);

        var appSettingsSection =
            _configuration.GetSection("JwtSettings");

        var jwtSettings =
            appSettingsSection.Get<JwtSettings>();

        var jwtResponse =
            new JwtService(jwtSettings).CreateToken(userExist);

        _outputPort.Ok(jwtResponse);
    }

    public void SetOutputPort(
        IOutputPortWithForbid<JwtResponse> outputPort)
        => _outputPort = outputPort;

    private static bool Verify(
        string password,
        string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}