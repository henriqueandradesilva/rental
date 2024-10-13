using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public abstract class BaseEntity
{
    public BaseEntity()
    {

    }

    public BaseEntity(
        long id)
    {
        Id = id;
    }

    [Key]
    public long Id { get; set; }

    private DateTime _dateCreated;
    public DateTime DateCreated
    {
        get { return _dateCreated; }
        set { _dateCreated = value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value, DateTimeKind.Utc); }
    }

    private DateTime? _dateUpdated;
    public DateTime? DateUpdated
    {
        get { return _dateUpdated; }
        set { _dateUpdated = value.HasValue ? (value.Value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)) : value; }
    }

    #region Methods

    public abstract bool Valid();

    public abstract bool Invalid();

    public abstract List<ValidationFailure> GetListNotification();

    #endregion

    #region Extensions

    public bool Valid<T>(
        T obj,
        AbstractValidator<T> validador)
    {
        var result = validador.Validate(obj);

        return result.IsValid;
    }

    public List<ValidationFailure> GetListNotification<T>(
        T obj,
        AbstractValidator<T> validador)
    {
        var result = validador.Validate(obj);

        return result.Errors;
    }

    public void SetDateCreated()
    {
        DateCreated = DateTime.UtcNow;
    }

    public void SetDateUpdated()
    {
        DateUpdated = DateTime.UtcNow;
    }

    #endregion
}