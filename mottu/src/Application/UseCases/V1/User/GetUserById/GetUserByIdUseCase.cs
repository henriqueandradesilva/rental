using Application.UseCases.V1.User.GetUserById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.User.GetUserById;

public class GetUserByIdUseCase : IGetUserByIdUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.User> _outputPort;
    private readonly IUserRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetUserByIdUseCase(
        IUserRepository repository,
        NotificationHelper notificationHelper)
    {
        _repository = repository;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        long id)
    {
        var result =
            await _repository?.Where(c => c.Id == id)
                             ?.Include(c => c.UserRole)
                             ?.Include(c => c.Driver)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.UserNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.User> outputPort)
        => _outputPort = outputPort;
}