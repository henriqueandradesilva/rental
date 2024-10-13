using Application.Services.Interfaces;
using Application.UseCases.V1.DriverNotificated.PutDriverNotificated.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.DriverNotificated.PutDriverNotificated;

public class PutDriverNotificatedUseCase : IPutDriverNotificatedUseCase
{
    private IOutputPort<Domain.Entities.DriverNotificated> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDriverNotificatedRepository _repository;
    private IDriverNotificatedService _driverNotificatedService;
    private readonly NotificationHelper _notificationHelper;

    public PutDriverNotificatedUseCase(
        IUnitOfWork unitOfWork,
        IDriverNotificatedRepository repository,
        IDriverNotificatedService driverNotificatedService,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _driverNotificatedService = driverNotificatedService;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.DriverNotificated driverNotificated)
    {
        var result =
           await _repository?.Where(c => c.Id != driverNotificated.Id &&
                                         c.DriverId == driverNotificated.DriverId &&
                                         c.NotificationId == driverNotificated.NotificationId)
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.DriverNotificatedExist);

            _outputPort.Error();
        }
        else
        {
            result =
                await _repository?.Where(c => c.Id == driverNotificated.Id)
                                 ?.FirstOrDefaultAsync();

            if (result == null)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.DriverNotificatedNotExist);

                _outputPort.Error();
            }
            else
            {
                driverNotificated =
                    await _driverNotificatedService.Init(_outputPort,
                                                         _notificationHelper,
                                                         driverNotificated);

                if (driverNotificated == null)
                    return;

                driverNotificated.Map(result);

                result.SetDateUpdated();

                _repository.Update(result);

                var response =
                    await _unitOfWork.Save()
                                     .ConfigureAwait(false);

                if (!string.IsNullOrEmpty(response))
                {
                    _notificationHelper.Add(SystemConst.Error, response);

                    _outputPort.Error();
                }
                else
                    _outputPort.Ok(result);
            }
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.DriverNotificated> outputPort)
        => _outputPort = outputPort;
}