using Application.Services.Interfaces;
using Application.UseCases.V1.Driver.PutDriverSetCnhImage.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services;

public class DriverService : IDriverService
{
    private readonly IPutDriverSetCnhImageUseCase _putDriverSetCnhImageUseCase;
    private readonly IDriverRepository _driverRepository;

    public DriverService(
        IPutDriverSetCnhImageUseCase putDriverSetCnhImageUseCase,
        IDriverRepository driverRepository)
    {
        _putDriverSetCnhImageUseCase = putDriverSetCnhImageUseCase;
        _driverRepository = driverRepository;
    }

    public async Task<bool> SetCnhImage(
        IOutputPort<Driver> outputPort,
        NotificationHelper notificationHelper,
        long driverId,
        string cnhBase64)
    {
        var driverOutputPort =
            new OutputPortService<Driver>(notificationHelper);

        _putDriverSetCnhImageUseCase.SetOutputPort(driverOutputPort);

        await _putDriverSetCnhImageUseCase.Execute(driverId, cnhBase64);

        if (driverOutputPort.Errors.Any())
        {
            if (!notificationHelper.HasMessage)
            {
                foreach (var error in driverOutputPort.Errors)
                    notificationHelper.Add(SystemConst.Error, error);
            }

            outputPort.Error();
        }

        return driverOutputPort?.Result?.Id > 0;
    }
}