using Application.Services.Interfaces;
using Application.UseCases.V1.Driver.PostDriver.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.PostDriver;

public class PostDriverUseCase : IPostDriverUseCase
{
    private IOutputPort<Domain.Entities.Driver> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDriverRepository _repository;
    private readonly IUserService _userService;
    private readonly IDriverService _driverService;
    private readonly NotificationHelper _notificationHelper;

    public PostDriverUseCase(
        IUnitOfWork unitOfWork,
        IDriverRepository repository,
        IUserService userService,
        IDriverService driverService,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _userService = userService;
        _driverService = driverService;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.Driver driver,
        string cnhBase64)
    {
        var userId =
            await _userService.CheckUserExist(_outputPort, _notificationHelper, driver.Name, driver.Cnh);

        if (userId == 0)
            return;

        var normalizedIdentifier = driver.Identifier?.NormalizeString();

        var normalizedCnpj = driver.Cnpj?.NormalizeString();

        var normalizedCnh = driver.Cnh?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Identifier.ToUpper().Trim().Contains(normalizedIdentifier) ||
                                         c.Cnpj.ToUpper().Trim().Contains(normalizedCnpj) ||
                                         c.Cnh.ToUpper().Trim().Contains(normalizedCnh))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.DriverExist);

            _outputPort.Error();
        }
        else if (driver.Id == 0)
        {
            driver.SetDateCreated();

            driver.SetUserId(userId);

            await _repository.Add(driver)
                             .ConfigureAwait(false);

            var response =
                await _unitOfWork.Save()
                                 .ConfigureAwait(false);

            if (!string.IsNullOrEmpty(response))
            {
                _notificationHelper.Add(SystemConst.Error, response);

                _outputPort.Error();
            }
            else
            {
                if (!string.IsNullOrEmpty(cnhBase64))
                {
                    var setCnhImageResponse =
                        await _driverService.SetCnhImage(_outputPort, _notificationHelper, driver.Id, cnhBase64);

                    if (!setCnhImageResponse)
                        return;
                }

                var setDriverResponse =
                     await _userService.SetDriverId(_outputPort, _notificationHelper, userId, driver.Id);

                if (!setDriverResponse)
                    return;

                _outputPort.Ok(driver);
            }
        }
        else
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

            _outputPort.Error();
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Driver> outputPort)
        => _outputPort = outputPort;
}