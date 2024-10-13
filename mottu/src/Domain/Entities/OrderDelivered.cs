using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class OrderDelivered : BaseEntity
{
    public OrderDelivered()
    {

    }

    public OrderDelivered(
        long id,
        long driverId,
        long orderId,
        DateTime date) : base(id)
    {
        DriverId = driverId;
        OrderId = orderId;
        Date = date;
    }

    public long DriverId { get; private set; }

    public long OrderId { get; private set; }

    public DateTime Date { get; private set; }

    public virtual Driver Driver { get; private set; }

    public virtual Order Order { get; private set; }

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<OrderDelivered> _validator
        = new Validators.ValidatorOrderDelivered();

    #endregion
}