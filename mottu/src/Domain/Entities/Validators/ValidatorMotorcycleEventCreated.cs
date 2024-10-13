using Domain.Common.Consts;
using FluentValidation;

namespace Domain.Entities.Validators;

public class ValidatorMotorcycleEventCreated : AbstractValidator<MotorcycleEventCreated>
{
    public ValidatorMotorcycleEventCreated()
    {
        RuleFor(m => m.MotorcycleId)
            .GreaterThan(0).WithMessage(MessageConst.MotorcycleEventCreatedMotorcycleIdRequired);

        RuleFor(m => m.Json)
            .NotNull().NotEmpty().WithName(MessageConst.MotorcycleEventCreatedJsonRequired);
    }
}