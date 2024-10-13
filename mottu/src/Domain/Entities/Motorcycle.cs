using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Domain.Entities;

public class Motorcycle : BaseEntity
{
    public Motorcycle()
    {

    }

    public Motorcycle(
         long id,
         string plate) : base(id)
    {
        Plate = plate?.ToUpper();
    }

    public Motorcycle(
        long id,
        string identifier,
        int year,
        string plate) : base(id)
    {
        Identifier = identifier;
        Year = year;
        Plate = plate?.ToUpper();
    }

    public long ModelVehicleId { get; private set; }

    public string Identifier { get; private set; }

    public int Year { get; private set; }

    public string Plate { get; private set; }

    public bool IsRented { get; private set; }

    public virtual ModelVehicle ModelVehicle { get; private set; }

    public virtual ICollection<Rental> ListRental { get; private set; } = new HashSet<Rental>();

    public virtual ICollection<MotorcycleEventCreated> ListMotorcycleEventCreated { get; private set; } = new HashSet<MotorcycleEventCreated>();

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<Motorcycle> _validator
        = new Validators.ValidatorMotorcycle();

    #endregion

    #region Extensions

    public void SetModelVehicleId(
        long modelVehicleId)
    {
        ModelVehicleId = modelVehicleId;
    }

    public void SetRented(
        bool isRetend)
    {
        IsRented = isRetend;
    }

    public void SetPlate(
        string plate)
    {
        if (!string.IsNullOrEmpty(plate))
            Plate = plate;
    }

    #endregion
}