using Domain.Common.Consts;
using FluentValidation;

namespace Domain.Entities.Validators;

public class ValidatorNotification : AbstractValidator<Notification>
{
    public ValidatorNotification()
    {
        RuleFor(u => u.OrderId)
            .GreaterThan(0).WithMessage(MessageConst.NotificationOrderIdRequired);

        RuleFor(u => u.Date)
            .NotNull().WithMessage(MessageConst.NotificationDateRequired)
            .Must(date => date != default).WithMessage(MessageConst.NotificationDateInvalid);
    }
}