using Application.Services.Interfaces;
using Application.UseCases.V1.Driver.PutDriverSetDelivering.Interfaces;
using Application.UseCases.V1.Motorcycle.PutMotorcycleSetRented.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Common.Consts;
using Domain.Common.Enums;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services;

public class RentalService : IRentalService
{
    private readonly IPutMotorcycleSetRentedUseCase _putMotorcycleSetRentedUseCase;
    private readonly IPutDriverSetDeliveringUseCase _putDriverSetDeliveringUseCase;
    private IMotorcycleRepository _motorcycleRepository;
    private IDriverRepository _driverRepository;
    private IPlanRepository _planRepository;

    public RentalService(
        IPutMotorcycleSetRentedUseCase putMotorcycleSetRentedUseCase,
        IPutDriverSetDeliveringUseCase putDriverSetDeliveringUseCase,
        IMotorcycleRepository motorcycleRepository,
        IDriverRepository driverRepository,
        IPlanRepository planRepository)
    {
        _putMotorcycleSetRentedUseCase = putMotorcycleSetRentedUseCase;
        _putDriverSetDeliveringUseCase = putDriverSetDeliveringUseCase;
        _motorcycleRepository = motorcycleRepository;
        _driverRepository = driverRepository;
        _planRepository = planRepository;
    }

    public async Task<Rental> Init(
        IOutputPort<Rental> outputPort,
        NotificationHelper notificationHelper,
        Rental rental,
        bool checkMotorcycleIsRented)
    {
        #region Motorcycle

        var motorcycle =
           await _motorcycleRepository?.Where(c => c.Id == rental.MotorcycleId)
                                      ?.FirstOrDefaultAsync();

        if (motorcycle == null)
        {
            notificationHelper.Add(SystemConst.Error, MessageConst.MotorcycleNotExist);

            outputPort.Error();

            return null;
        }

        if (checkMotorcycleIsRented)
        {
            if (motorcycle.IsRented)
            {
                notificationHelper.Add(SystemConst.Error, MessageConst.MotorcycleIsRented);

                outputPort.Error();

                return null;
            }
        }

        #endregion

        #region Driver

        var driver =
           await _driverRepository?.Where(c => c.Id == rental.DriverId)
                                  ?.Include(c => c.User)
                                  ?.FirstOrDefaultAsync();

        if (driver == null)
        {
            notificationHelper.Add(SystemConst.Error, MessageConst.DriverNotExist);

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

        #endregion

        #region Plan

        var plan =
           await _planRepository?.Where(c => c.Id == rental.PlanId)
                                ?.FirstOrDefaultAsync();

        if (plan == null)
        {
            notificationHelper.Add(SystemConst.Error, MessageConst.PlanNotExist);

            outputPort.Error();

            return null;
        }

        if (!plan.IsActive)
        {
            notificationHelper.Add(SystemConst.Error, MessageConst.PlanIsNotActive);

            outputPort.Error();

            return null;
        }

        #endregion

        var isRented =
            await CheckMotorcycleRented(outputPort, notificationHelper, rental.MotorcycleId, true);

        if (!isRented)
            return null;

        rental = CalculateBaseTotal(rental, plan);

        return rental;
    }

    public async Task<Rental> Finish(
        IOutputPort<Rental> outputPort,
        NotificationHelper notificationHelper,
        Rental rental)
    {
        // Calculate the total duration in days
        TimeSpan duration = rental.EndDate - rental.StartDate;
        int totalDayDuration = (int)Math.Ceiling(duration.TotalDays);

        // Set the allocated period and the expected end date
        rental.SetAllocatePeriod(totalDayDuration);

        // Initialize the total amount with zero
        double totalAmount = 0;

        var plan = rental.Plan;

        if (totalDayDuration < plan.DurationInDays)
        {
            // If days used are less than the plan duration
            double missingDays = plan.DurationInDays - totalDayDuration;

            totalAmount = totalDayDuration * plan.DailyRate; // Cost for days used

            totalAmount += missingDays * plan.DailyRate * plan.DailyLateFee; // Penalty for unused days
        }
        else
        {
            // If days used are greater than the plan duration
            totalAmount = plan.DurationInDays * plan.DailyRate; // Cost for the full plan duration

            if (totalDayDuration > plan.DurationInDays)
            {
                double additionalDays = totalDayDuration - plan.DurationInDays;

                totalAmount += additionalDays * plan.AdditionalRate; // Cost for additional days
            }
        }

        // Set the final total amount
        rental.SetTotalAmount(Math.Round(totalAmount, 2));

        var isRented =
            await CheckMotorcycleRented(outputPort, notificationHelper, rental.MotorcycleId, false);

        if (!isRented)
            return null;

        rental.Motorcycle.SetRented(isRented);
        rental.SetStatus(RentalStatusEnum.Return);

        return rental;
    }

    private Rental CalculateBaseTotal(
        Rental rental,
        Plan plan)
    {
        // Set end date
        var endDate = rental.StartDate.AddDays(plan.DurationInDays);

        // Set the allocated period and the expected end date
        rental.SetAllocatePeriod(plan.DurationInDays);
        rental.SetEndDate(endDate);
        rental.SetExpectedEndDate(endDate);

        var totalAmount = plan.DurationInDays * plan.DailyRate; // Cost for the full plan duration

        rental.SetTotalAmount(Math.Round(totalAmount, 2));

        return rental;
    }

    private async Task<bool> CheckMotorcycleRented(
        IOutputPort<Rental> outputPort,
        NotificationHelper notificationHelper,
        long motocycleId,
        bool rented)
    {
        var motorcycleOutputPort =
            new OutputPortService<Motorcycle>(notificationHelper);

        _putMotorcycleSetRentedUseCase.SetOutputPort(motorcycleOutputPort);

        await _putMotorcycleSetRentedUseCase.Execute(motocycleId, rented);

        if (motorcycleOutputPort.Errors.Any())
        {
            if (!notificationHelper.HasMessage)
            {
                foreach (var error in motorcycleOutputPort.Errors)
                    notificationHelper.Add(SystemConst.Error, error);
            }

            outputPort.Error();
        }

        return motorcycleOutputPort?.Result?.Id > 0;
    }
}