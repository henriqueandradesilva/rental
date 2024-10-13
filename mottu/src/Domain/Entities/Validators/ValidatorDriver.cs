using Domain.Common.Consts;
using Domain.Common.Extensions;
using FluentValidation;
using System;

namespace Domain.Entities.Validators;

public class ValidatorDriver : AbstractValidator<Driver>
{
    public ValidatorDriver()
    {
        RuleFor(d => d.Identifier)
            .NotNull().NotEmpty().WithMessage(MessageConst.DriverIdentifierRequired);

        RuleFor(d => d.Name)
            .NotNull().NotEmpty().WithMessage(MessageConst.DriverNameRequired)
            .MaximumLength(150).WithMessage(MessageConst.DriverNameMaxPermitted);

        RuleFor(d => d.Cnpj)
            .NotEmpty().WithMessage(MessageConst.DriverCnpjRequired)
            .Must(ValidateExtension.IsCnpjValid).WithMessage(MessageConst.DriverCnpjInvalid);

        RuleFor(d => d.Cnh)
            .NotEmpty().WithMessage(MessageConst.DriverCnhRequired)
            .Must(ValidateExtension.IsCnhValid).WithMessage(MessageConst.DriverCnhInvalid);

        RuleFor(d => d.Type)
            .IsInEnum().WithMessage(MessageConst.DriverCnhTypeInvalid);

        RuleFor(d => d.DateOfBirth)
            .NotNull().WithMessage(MessageConst.DriverDateOfBirthRequired)
            .Must(date => date != DateOnly.MinValue).WithMessage(MessageConst.DriverDateOfBirthInvalid)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18))).WithMessage($"{MessageConst.DriverDateOfBirthRuleInvalid} {DateTime.UtcNow.AddYears(-18).ToShortDateString()}.");
    }
}