using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Order : BaseEntity
{
    public Order()
    {

    }

    public Order(
        long id,
        long statusId,
        string description,
        double value,
        DateTime date) : base(id)
    {
        StatusId = statusId;
        Description = description?.ToUpper();
        Value = value;
        Date = date;
    }

    public long StatusId { get; private set; }

    public string Description { get; private set; }

    public double Value { get; private set; }

    public DateTime Date { get; private set; }

    public virtual OrderStatus Status { get; private set; }

    public virtual OrderAccepted Accepted { get; private set; }

    public virtual OrderDelivered Delivered { get; private set; }

    public virtual ICollection<Notification> ListNotification { get; private set; } = new HashSet<Notification>();

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<Order> _validator
        = new Validators.ValidatorOrder();

    #endregion

    #region Extensions

    public void SetOrderStatus(
        long orderStatusId)
    {
        StatusId = orderStatusId;
    }

    #endregion
}