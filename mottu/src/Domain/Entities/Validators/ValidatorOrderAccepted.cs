using Domain.Common.Consts;
using FluentValidation;

namespace Domain.Entities.Validators;

public class ValidatorOrderAccepted : AbstractValidator<OrderAccepted>
{
    public ValidatorOrderAccepted()
    {
        RuleFor(u => u.DriverId)
            .GreaterThan(0).WithMessage(MessageConst.OrderAcceptedDriverIdRequired);

        RuleFor(u => u.OrderId)
            .GreaterThan(0).WithMessage(MessageConst.OrderAcceptedOrderIdRequired);

        RuleFor(u => u.Date)
            .NotNull().WithMessage(MessageConst.OrderAcceptedDateRequired)
            .Must(date => date != default).WithMessage(MessageConst.OrderAcceptedDateInvalid);
    }
}