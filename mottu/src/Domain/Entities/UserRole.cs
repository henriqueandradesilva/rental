using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Domain.Entities;

public class UserRole : BaseEntity
{
    public UserRole()
    {

    }

    public UserRole(
        long id,
        string name) : base(id)
    {
        Description = name?.ToUpper();
    }

    public string Description { get; private set; }

    public virtual ICollection<User> ListUser { get; private set; } = new HashSet<User>();

    #region Validate

    public override bool Valid()
        => Valid(this, _validator);

    public override bool Invalid()
        => !Valid(this, _validator);

    public override List<ValidationFailure> GetListNotification()
        => GetListNotification(this, _validator);

    private readonly AbstractValidator<UserRole> _validator
        = new Validators.ValidatorUserRole();

    #endregion
}