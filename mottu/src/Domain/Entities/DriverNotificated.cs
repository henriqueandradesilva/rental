using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class DriverNotificated : BaseEntity
{
    public DriverNotificated()
    {

    }

    public DriverNotificated(
        long id,
        long driverId,
        long notificationId,
        DateTime date) : base(id)
    {
        DriverId = driverId;
        NotificationId = notificationId;
        Date = date;
    }

    public long DriverId { get; private set; }

    public long NotificationId { get; private set; }

    public DateTime Date { get; private set; }

    public virtual Driver Driver { get; private set; }

    public virtual Notification Notification { get; private set; }

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<DriverNotificated> _validator
        = new Validators.ValidatorDriverNotificated();

    #endregion
}