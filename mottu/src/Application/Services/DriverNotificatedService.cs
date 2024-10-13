using Application.Services.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Common.Enums;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.Services;

public class DriverNotificatedService : IDriverNotificatedService
{
    private IDriverRepository _driverRepository;

    public DriverNotificatedService(
        IDriverRepository driverRepository)
    {
        _driverRepository = driverRepository;
    }

    public async Task<DriverNotificated> Init(
        IOutputPort<DriverNotificated> outputPort,
        NotificationHelper notificationHelper,
        DriverNotificated driverNotificated)
    {
        var driver =
           await _driverRepository?.Where(c => c.Id == driverNotificated.DriverId)
                                  ?.Include(c => c.User)
                                  ?.FirstOrDefaultAsync();

        if (driver == null)
        {
            notificationHelper.Add(SystemConst.Error, MessageConst.DriverNotExist);

            outputPort.Error();

            return null;
        }

        if (driver.Delivering)
        {
            notificationHelper.Add(SystemConst.Error, MessageConst.DriverIsDelivering);

            outputPort.Error();

            return null;
        }

        if (!driver.User.IsActive)
        {
            notificationHelper.Add(SystemConst.Error, MessageConst.UserIsNotActive);

            outputPort.Error();

            return null;
        }

        if (driver.Type != CnhTypeEnum.A)
        {
            notificationHelper.Add(SystemConst.Error, MessageConst.RentalDriverNotCnhTypeA);

            outputPort.Error();

            return null;
        }

        return driverNotificated;
    }
}