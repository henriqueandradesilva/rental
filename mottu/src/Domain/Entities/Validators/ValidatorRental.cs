using Domain.Common.Consts;
using FluentValidation;
using System;

namespace Domain.Entities.Validators;

public class ValidatorRental : AbstractValidator<Rental>
{
    public ValidatorRental()
    {
        RuleFor(r => r.MotorcycleId)
            .GreaterThan(0).WithMessage(MessageConst.RentalMotorcycleIdRequired);

        RuleFor(r => r.DriverId)
            .GreaterThan(0).WithMessage(MessageConst.RentalDriverIdRequired);

        RuleFor(r => r.PlanId)
            .GreaterThan(0).WithMessage(MessageConst.RentalPlanIdRequired);

        RuleFor(r => r.StartDate)
            .NotNull().WithMessage(MessageConst.RentalStartDateRequired)
            .Must(date => date != default).WithMessage(MessageConst.RentalStartDateRequired)
            .GreaterThan(DateTime.UtcNow).WithMessage(MessageConst.RentalStartDateRuleInvalid);
    }
}