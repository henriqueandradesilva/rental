using Domain.Common.Consts;
using FluentValidation;

namespace Domain.Entities.Validators;

public class ValidatorOrderDelivered : AbstractValidator<OrderDelivered>
{
    public ValidatorOrderDelivered()
    {
        RuleFor(u => u.DriverId)
            .GreaterThan(0).WithMessage(MessageConst.OrderDeliveredDriverIdRequired);

        RuleFor(u => u.OrderId)
            .GreaterThan(0).WithMessage(MessageConst.OrderDeliveredOrderIdRequired);

        RuleFor(u => u.Date)
            .NotNull().WithMessage(MessageConst.OrderDeliveredDateRequired)
            .Must(date => date != default).WithMessage(MessageConst.OrderDeliveredDateInvalid);
    }
}