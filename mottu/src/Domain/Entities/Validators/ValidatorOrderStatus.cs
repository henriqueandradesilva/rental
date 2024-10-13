using Domain.Common.Consts;
using FluentValidation;

namespace Domain.Entities.Validators;

public class ValidatorOrderStatus : AbstractValidator<OrderStatus>
{
    public ValidatorOrderStatus()
    {
        RuleFor(u => u.Description)
            .NotNull().NotEmpty().WithMessage(MessageConst.DescriptionRequired)
            .MaximumLength(100).WithMessage(MessageConst.DescriptionMaxPermitted);
    }
}