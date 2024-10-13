using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Domain.Entities;

public class PlanType : BaseEntity
{
    public PlanType()
    {

    }

    public PlanType(
        long id,
        string description) : base(id)
    {
        Description = description?.ToUpper();
    }

    public string Description { get; private set; }

    public virtual ICollection<Plan> ListPlan { get; private set; } = new HashSet<Plan>();

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<PlanType> _validator
        = new Validators.ValidatorPlanType();

    #endregion
}