using Domain.Common.Consts;
using FluentValidation;

namespace Domain.Entities.Validators;

public class ValidatorDriverNotificated : AbstractValidator<DriverNotificated>
{
    public ValidatorDriverNotificated()
    {
        RuleFor(u => u.DriverId)
            .GreaterThan(0).WithMessage(MessageConst.DriverNotificatedDriverIdRequired);

        RuleFor(u => u.NotificationId)
            .GreaterThan(0).WithMessage(MessageConst.DriverNotificatedNotificationIdRequired);

        RuleFor(u => u.Date)
            .NotNull().WithMessage(MessageConst.DriverNotificatedDateRequired)
            .Must(date => date != default).WithMessage(MessageConst.DriverNotificatedDateInvalid);
    }
}