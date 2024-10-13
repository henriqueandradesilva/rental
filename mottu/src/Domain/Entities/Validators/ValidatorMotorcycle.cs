using Domain.Common.Consts;
using FluentValidation;

namespace Domain.Entities.Validators;

public class ValidatorMotorcycle : AbstractValidator<Motorcycle>
{
    public ValidatorMotorcycle()
    {
        RuleFor(m => m.Identifier)
            .NotNull().NotEmpty().WithName(MessageConst.MotorcycleIdentifierRequired)
            .MaximumLength(100).WithMessage(MessageConst.MotorcycleIdentifierMaxPermitted);

        RuleFor(m => m.Year)
            .GreaterThan(0).WithMessage(MessageConst.MotorcycleYearRequired);

        RuleFor(m => m.Plate)
            .NotNull().NotEmpty().WithName(MessageConst.MotorcyclePlateRequired)
            .MaximumLength(8).WithMessage(MessageConst.MotorcyclePlateMaxPermitted);
    }
}