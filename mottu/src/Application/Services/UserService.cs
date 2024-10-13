using Application.Services.Interfaces;
using Application.UseCases.V1.User.PostUser.Interfaces;
using Application.UseCases.V1.User.PutUser.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IPostUserUseCase _postUserUseCase;
    private readonly IPutUserUseCase _putUserUseCase;
    private readonly IUserRepository _userRepository;

    public UserService(
        IPostUserUseCase postUserUseCase,
        IPutUserUseCase putUserUseCase,
        IUserRepository userRepository)
    {
        _postUserUseCase = postUserUseCase;
        _putUserUseCase = putUserUseCase;
        _userRepository = userRepository;
    }

    public async Task<long> CheckUserExist(
        IOutputPort<Driver> outputPort,
        NotificationHelper notificationHelper,
        string userName,
        string cnh)
    {
        long userId = 0;

        var normalizedName =
            userName?.NormalizeString();

        var result =
           await _userRepository?.Where(c => c.Name.ToUpper().Trim().Contains(normalizedName))
                                ?.FirstOrDefaultAsync();

        if (result != null)
        {
            userId = result.Id;

            return userId;
        }

        return await CreateUser(outputPort, notificationHelper, userName, cnh);
    }

    public async Task<bool> SetDriverId(
        IOutputPort<Driver> outputPort,
        NotificationHelper notificationHelper,
        long userId,
        long driverId)
    {
        var result =
            await _userRepository?.Where(c => c.Id == userId)
                                 ?.FirstOrDefaultAsync();

        if (result != null)
        {
            var userOutputPort =
                new OutputPortService<User>(notificationHelper);

            _putUserUseCase.SetOutputPort(userOutputPort);

            await _putUserUseCase.Execute(result);

            if (userOutputPort.Errors.Any())
            {
                if (!notificationHelper.HasMessage)
                {
                    foreach (var error in userOutputPort.Errors)
                        notificationHelper.Add(SystemConst.Error, error);
                }

                outputPort.Error();
            }

            return userOutputPort?.Result?.Id > 0;
        }

        return false;
    }

    private async Task<long> CreateUser(
        IOutputPort<Driver> outputPort,
        NotificationHelper notificationHelper,
        string userName,
        string cnh)
    {
        var email = $"{userName}@mottu.app";

        var password = cnh.Substring(cnh.Length - 4);

        var user =
            new User(0, SystemConst.UserRoleDriverIdDefault, userName, email, password, false);

        var userOutputPort =
            new OutputPortService<User>(notificationHelper);

        _postUserUseCase.SetOutputPort(userOutputPort);

        await _postUserUseCase.Execute(user);

        if (userOutputPort.Errors.Any())
        {
            if (!notificationHelper.HasMessage)
            {
                foreach (var error in userOutputPort.Errors)
                    notificationHelper.Add(SystemConst.Error, error);
            }

            outputPort.Error();
        }

        return userOutputPort.Result?.Id ?? 0;
    }
}