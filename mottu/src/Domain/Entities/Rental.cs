using Domain.Common.Enums;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Rental : BaseEntity
{
    public Rental()
    {

    }

    public Rental(
        long id,
        long motorcycleId,
        long driverId,
        long planId,
        DateTime startDate,
        RentalStatusEnum rentalStatus) : base(id)
    {
        MotorcycleId = motorcycleId;
        DriverId = driverId;
        PlanId = planId;
        StartDate = startDate;
        Status = rentalStatus;
    }

    public long MotorcycleId { get; private set; }

    public long DriverId { get; private set; }

    public long PlanId { get; private set; }

    public int AllocatePeriod { get; private set; }

    public double TotalAmount { get; private set; }

    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }

    public DateTime ExpectedEndDate { get; private set; }

    public RentalStatusEnum Status { get; private set; }

    public virtual Motorcycle Motorcycle { get; private set; }

    public virtual Driver Driver { get; private set; }

    public virtual Plan Plan { get; private set; }

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<Rental> _validator
        = new Validators.ValidatorRental();

    #endregion

    #region Extensions

    public void SetAllocatePeriod(
        int allocatePeriod)
    {
        AllocatePeriod = allocatePeriod;
    }

    public void SetEndDate(
        DateTime endDate)
    {
        EndDate = endDate;
    }

    public void SetExpectedEndDate(
        DateTime expectedEndDate)
    {
        ExpectedEndDate = expectedEndDate;
    }

    public void SetTotalAmount(
        double totalAmount)
    {
        TotalAmount = totalAmount;
    }

    public void SetStatus(
        RentalStatusEnum status)
    {
        Status = status;
    }

    #endregion
}