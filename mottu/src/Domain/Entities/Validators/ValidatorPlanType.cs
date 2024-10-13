using Domain.Common.Consts;
using FluentValidation;

namespace Domain.Entities.Validators;

public class ValidatorPlanType : AbstractValidator<PlanType>
{
    public ValidatorPlanType()
    {
        RuleFor(p => p.Description)
            .NotNull().NotEmpty().WithMessage(MessageConst.DescriptionRequired)
            .MaximumLength(100).WithMessage(MessageConst.DescriptionMaxPermitted);
    }
}