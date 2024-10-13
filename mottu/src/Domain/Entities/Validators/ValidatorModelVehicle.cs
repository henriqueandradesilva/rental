using Domain.Common.Consts;
using FluentValidation;

namespace Domain.Entities.Validators;

public class ValidatorModelVehicle : AbstractValidator<ModelVehicle>
{
    public ValidatorModelVehicle()
    {
        RuleFor(m => m.Description)
            .NotNull().NotEmpty().WithMessage(MessageConst.DescriptionRequired)
            .MaximumLength(100).WithMessage(MessageConst.DescriptionMaxPermitted);
    }
}