using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Domain.Entities;

public class Plan : BaseEntity
{
    public Plan()
    {

    }

    public Plan(
        long id,
        long planTypeId,
        string description,
        double dailyRate,
        double dailyLateFee,
        double additionalCharge,
        int durationInDays,
        bool isActive) : base(id)
    {
        PlanTypeId = planTypeId;
        Description = description?.ToUpper();
        DailyRate = dailyRate;
        DailyLateFee = dailyLateFee;
        AdditionalRate = additionalCharge;
        DurationInDays = durationInDays;
        IsActive = isActive;
    }

    public long PlanTypeId { get; private set; }

    public string Description { get; private set; }

    public double DailyRate { get; private set; }

    public double AdditionalRate { get; private set; }

    public double DailyLateFee { get; private set; }

    public int DurationInDays { get; private set; }

    public bool IsActive { get; private set; }

    public virtual PlanType PlanType { get; private set; }

    public virtual ICollection<Rental> ListRental { get; private set; } = new HashSet<Rental>();

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<Plan> _validator
        = new Validators.ValidatorPlan();

    #endregion

    #region Extensions

    public void SetActive(
        bool isActive)
    {
        IsActive = isActive;
    }

    #endregion
}