using Application.Services.Interfaces;
using Application.UseCases.V1.Rental.PutRentalSetReturn.Interfaces;
using CrossCutting.Const;
using CrossCutting.Extensions.UseCases;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Rental.PutRentalSetReturn;

public class PutRentalSetReturnUseCase : IPutRentalSetReturnUseCase
{
    private IOutputPort<Domain.Entities.Rental> _outputPort;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRentalRepository _repository;
    private IRentalService _rentalService;
    private readonly NotificationHelper _notificationHelper;

    public PutRentalSetReturnUseCase(
        IUnitOfWork unitOfWork,
        IRentalRepository repository,
        IRentalService rentalService,
        NotificationHelper notificationHelper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _rentalService = rentalService;
        _notificationHelper = notificationHelper;
    }

    public async Task Execute(
        long id,
        DateTime endDate,
        ClaimsPrincipal user)
    {
        var result =
            await _repository?.Where(c => c.Id == id)
                             ?.Include(c => c.Motorcycle)
                             ?.Include(c => c.Driver)
                             ?.Include(c => c.Plan)
                             ?.FirstOrDefaultAsync();

        if (result == null)
        {
            _notificationHelper.Add(SystemConst.Error, MessageConst.RentalNotExist);

            _outputPort.Error();
        }
        else
        {
            if (result.Status == Domain.Common.Enums.RentalStatusEnum.Return)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.RentalIsReturn);

                _outputPort.Error();

                return;
            }

            if (!ValidateUserExtension.IsOwnerOrAdminByDriverId(result.DriverId, user))
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.ActionNotPermitted);

                _outputPort.Error();

                return;
            }

            if (endDate == default)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.RentalEndDateInvalid);

                _outputPort.Error();

                return;
            }

            if (endDate <= result.StartDate)
            {
                _notificationHelper.Add(SystemConst.Error, MessageConst.RentalEndDateMustBeAfterStartDate);

                _outputPort.Error();

                return;
            }

            result.SetEndDate(endDate);

            result =
              await _rentalService.Finish(_outputPort, _notificationHelper, result);

            if (result == null)
                return;

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

    public void SetOutputPort(
        IOutputPort<Domain.Entities.Rental> outputPort)
        => _outputPort = outputPort;
}