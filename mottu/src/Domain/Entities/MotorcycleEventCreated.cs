using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Domain.Entities;

public class MotorcycleEventCreated : BaseEntity
{
    public MotorcycleEventCreated()
    {

    }

    public MotorcycleEventCreated(
        long id,
        long motorcycleId,
        string json) : base(id)
    {
        MotorcycleId = motorcycleId;
        Json = json;
    }

    public long MotorcycleId { get; private set; }

    public string Json { get; private set; }

    public bool CurrentYear { get; private set; }

    public virtual Motorcycle Motorcycle { get; private set; }

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<MotorcycleEventCreated> _validator
        = new Validators.ValidatorMotorcycleEventCreated();

    #endregion

    #region Extensions

    public void AddCurrentYear(
        bool currentYear)
    {
        CurrentYear = currentYear;
    }

    #endregion
}