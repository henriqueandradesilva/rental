using Domain.Common.Enums;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Driver : BaseEntity
{
    public Driver()
    {

    }

    public Driver(
        long id,
        string identifier,
        string cnpj,
        string name,
        DateTime dateOfBirth,
        string cnh,
        string cnhType) : base(id)
    {
        Identifier = identifier;
        Cnpj = cnpj?.ToUpper().Replace(".", "").Replace("/", "").Replace("-", "").Trim();
        Name = name?.ToUpper();
        DateOfBirth = DateOnly.FromDateTime(dateOfBirth);
        Cnh = cnh?.ToUpper().Replace(".", "").Replace("/", "").Replace("-", "").Trim();
        Type = Enum.TryParse(cnhType, out CnhTypeEnum cnhTypeEnum) ? cnhTypeEnum : CnhTypeEnum.None;
    }

    public Driver(
        long id,
        long? userId,
        string identifier,
        string name,
        string cnpj,
        string cnh,
        CnhTypeEnum cnhType,
        DateOnly dateOfBirth) : base(id)
    {
        UserId = userId;
        Identifier = identifier;
        Name = name?.ToUpper();
        Cnpj = cnpj?.ToUpper().Replace(".", "").Replace("/", "").Replace("-", "").Trim();
        Cnh = cnh?.ToUpper().Replace(".", "").Replace("/", "").Replace("-", "").Trim();
        Type = cnhType;
        DateOfBirth = dateOfBirth;
    }

    public long? UserId { get; private set; }

    public string Identifier { get; private set; }

    public string Name { get; private set; }

    public string Cnpj { get; private set; }

    public string Cnh { get; private set; }

    public string CnhImageUrl { get; private set; }

    public CnhTypeEnum Type { get; private set; }

    public bool Delivering { get; private set; }

    public DateOnly DateOfBirth { get; private set; }

    public virtual User User { get; private set; }

    public virtual ICollection<OrderAccepted> ListAccepted { get; private set; } = new HashSet<OrderAccepted>();

    public virtual ICollection<OrderDelivered> ListDelivered { get; private set; } = new HashSet<OrderDelivered>();

    public virtual ICollection<DriverNotificated> ListDriverNotificated { get; private set; } = new HashSet<DriverNotificated>();

    public virtual ICollection<Rental> ListRental { get; private set; } = new HashSet<Rental>();

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<Driver> _validator
        = new Validators.ValidatorDriver();

    #endregion

    #region Extensions

    public void SetCnhImageUrl(
        string cnhImageUrl)
    {
        if (!string.IsNullOrEmpty(cnhImageUrl))
            CnhImageUrl = cnhImageUrl;
    }

    public void SetUserId(
        long userId)
    {
        UserId = userId;
    }

    public void SetDelivering(
        bool delivering)
    {
        Delivering = delivering;
    }

    #endregion
}