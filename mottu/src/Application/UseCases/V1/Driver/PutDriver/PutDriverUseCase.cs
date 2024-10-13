using Application.Services.Interfaces;
using Application.UseCases.V1.Driver.PutDriver.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Driver.PutDriver;

public class PutDriverUseCase : IPutDriverUseCase
{
    private IOutputPort<Domain.Entities.Driver> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDriverRepository _repository;
    private readonly IDriverService _driverService;
    private readonly NotificationHelper _notificationHelper;

    public PutDriverUseCase(
        IUnitOfWork unitOfWork,
        IDriverRepository repository,
        IDriverService driverService,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _driverService = driverService;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        Domain.Entities.Driver driver,
        string cnhBase64)
    {
        var normalizedIdentifier = driver.Identifier?.NormalizeString();

        var normalizedCnpj = driver.Cnpj?.NormalizeString();

        var normalizedCnh = driver.Cnh?.NormalizeString();

        var result =
           await _repository?.Where(c => c.Id != driver.Id &&
                                         (c.Identifier.ToUpper().Trim().Contains(normalizedIdentifier) ||
                                          c.Cnpj.ToUpper().Trim().Contains(normalizedCnpj) ||
                                          c.Cnh.ToUpper().Trim().Contains(normalizedCnh)
                                         ))
                            ?.FirstOrDefaultAsync();

        if (result != null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.DriverExist);

            _outputPort.Error();
        }
        else
        {
            result =
                await _repository?.Where(c => c.Id == driver.Id)
                                 ?.FirstOrDefaultAsync();

            if (result == null)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.DriverNotExist);

                _outputPort.Error();
            }
            else
            {
                driver.Map(result);

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
                {
                    if (!string.IsNullOrEmpty(cnhBase64))
                    {
                        var setCnhImageResponse =
                            await _driverService.SetCnhImage(_outputPort, _notificationHelper, driver.Id, cnhBase64);

                        if (!setCnhImageResponse)
                            return;
                    }

                    _outputPort.Ok(result);
                }
            }
        }
    }

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Driver> outputPort)
        => _outputPort = outputPort;
}