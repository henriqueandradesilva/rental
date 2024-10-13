using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Domain.Entities;

public class ModelVehicle : BaseEntity
{
    public ModelVehicle()
    {

    }

    public ModelVehicle(
        long id,
        string description) : base(id)
    {
        Description = description?.ToUpper();
    }

    public string Description { get; private set; }

    public virtual ICollection<Motorcycle> ListMotorcycle { get; private set; } = new HashSet<Motorcycle>();

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<ModelVehicle> _validator
        = new Validators.ValidatorModelVehicle();

    #endregion
}