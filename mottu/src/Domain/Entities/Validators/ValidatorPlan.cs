using Domain.Common.Consts;
using FluentValidation;

namespace Domain.Entities.Validators;

public class ValidatorPlan : AbstractValidator<Plan>
{
    public ValidatorPlan()
    {
        RuleFor(p => p.PlanTypeId)
            .GreaterThan(0).WithMessage(MessageConst.PlanTypeIdRequired);

        RuleFor(p => p.Description)
            .NotNull().NotEmpty().WithMessage(MessageConst.DescriptionRequired)
            .MaximumLength(100).WithMessage(MessageConst.DescriptionMaxPermitted);

        RuleFor(p => p.DailyRate)
            .GreaterThan(0).WithMessage(MessageConst.PlanDailyRateRequired);

        RuleFor(p => p.AdditionalRate)
            .GreaterThan(0).WithMessage(MessageConst.PlanAdditionalRateRequired);

        RuleFor(p => p.DailyLateFee)
            .GreaterThan(0).WithMessage(MessageConst.PlanDailyLateFeeRequired);

        RuleFor(p => p.DurationInDays)
            .GreaterThan(0).WithMessage(MessageConst.PlanDurationInDaysRequired);
    }
}