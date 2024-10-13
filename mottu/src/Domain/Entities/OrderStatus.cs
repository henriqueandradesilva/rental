using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Domain.Entities;

public class OrderStatus : BaseEntity
{
    public OrderStatus()
    {

    }

    public OrderStatus(
        long id,
        string name) : base(id)
    {
        Description = name?.ToUpper();
    }

    public string Description { get; private set; }

    public virtual ICollection<Order> ListOrder { get; private set; } = new HashSet<Order>();

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<OrderStatus> _validator
        = new Validators.ValidatorOrderStatus();

    #endregion
}