using Domain.Common.Consts;
using FluentValidation;

namespace Domain.Entities.Validators;

public class ValidatorUser : AbstractValidator<User>
{
    public ValidatorUser()
    {
        RuleFor(r => r.UserRoleId)
            .GreaterThan(0).WithMessage(MessageConst.UserUserRoleIdRequired);

        RuleFor(u => u.Name)
            .NotNull().NotEmpty().WithMessage(MessageConst.UserNameRequired)
            .MaximumLength(100).WithMessage(MessageConst.UserNameMaxPermitted);

        RuleFor(u => u.Email)
            .NotNull().NotEmpty().WithMessage(MessageConst.UserEmailRequired)
            .MaximumLength(255).WithMessage(MessageConst.UserEmailMaxPermitted);

        RuleFor(u => u.Password)
            .NotNull().NotEmpty().WithMessage(MessageConst.UserPasswordRequired)
            .MaximumLength(255).WithMessage(MessageConst.UserPasswordMaxPermitted);
    }
}