using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Notification : BaseEntity
{
    public Notification()
    {

    }

    public Notification(
        long id,
        long orderId,
        DateTime date) : base(id)
    {
        OrderId = orderId;
        Date = date;
    }

    public long OrderId { get; private set; }

    public DateTime Date { get; private set; }

    public virtual Order Order { get; private set; }

    public virtual ICollection<DriverNotificated> ListDriverNotificated { get; private set; } = new HashSet<DriverNotificated>();

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<Notification> _validator
        = new Validators.ValidatorNotification();

    #endregion
}