using Application.UseCases.V1.DriverNotificated.GetDriverNotificatedById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.DriverNotificated.GetDriverNotificatedById;

public class GetDriverNotificatedByIdUseCase : IGetDriverNotificatedByIdUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.DriverNotificated> _outputPort;
    private readonly IDriverNotificatedRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetDriverNotificatedByIdUseCase(
        IDriverNotificatedRepository repository,
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
                             ?.Include(c => c.Driver)
                             ?.Include(c => c.Notification)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.DriverNotificatedNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.DriverNotificated> outputPort)
        => _outputPort = outputPort;
}