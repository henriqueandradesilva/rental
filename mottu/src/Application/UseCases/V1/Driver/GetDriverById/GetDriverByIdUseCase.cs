using Application.UseCases.V1.Driver.GetDriverById.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.GetDriverById;

public class GetDriverByIdUseCase : IGetDriverByIdUseCase
{
    private IOutputPortWithNotFound<Domain.Entities.Driver> _outputPort;
    private readonly IDriverRepository _repository;
    private readonly NotificationHelper _notificationHelper;

    public GetDriverByIdUseCase(
        IDriverRepository repository,
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
                             ?.Include(c => c.User)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.NotFound, MessageConst.DriverNotExist);

            _outputPort.NotFound();
        }
        else
            _outputPort.Ok(result);
    }

    public void SetOutputPort(
        IOutputPortWithNotFound<Domain.Entities.Driver> outputPort)
        => _outputPort = outputPort;
}