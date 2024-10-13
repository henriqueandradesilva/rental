using Domain.Common.Consts;
using FluentValidation;

namespace Domain.Entities.Validators;

public class ValidatorOrder : AbstractValidator<Order>
{
    public ValidatorOrder()
    {
        RuleFor(u => u.Description)
            .NotNull().NotEmpty().WithMessage(MessageConst.DescriptionRequired)
            .MaximumLength(100).WithMessage(MessageConst.DescriptionMaxPermitted);

        RuleFor(u => u.Value)
            .GreaterThan(0).WithMessage(MessageConst.OrderValueRequired);

        RuleFor(u => u.Date)
            .NotNull().WithMessage(MessageConst.OrderDateRequired)
            .Must(date => date != default).WithMessage(MessageConst.OrderDateInvalid);
    }
}