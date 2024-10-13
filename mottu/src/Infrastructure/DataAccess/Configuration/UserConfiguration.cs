using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Primary key
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
               .ValueGeneratedOnAdd();

        // Property Entity
        builder.Property(c => c.DateCreated)
               .HasColumnType("timestamp with time zone")
               .IsRequired()
               .HasDefaultValueSql("now()");

        builder.Property(c => c.DateUpdated)
               .HasColumnType("timestamp with time zone")
               .IsRequired(false);

        // Property
        builder.Property(c => c.Name)
               .HasColumnType("varchar(100)")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.Email)
               .HasColumnType("varchar(255)")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(c => c.Password)
               .HasColumnType("varchar(255)")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(c => c.IsActive)
               .HasColumnType("boolean")
               .HasDefaultValue(false);

        // Index
        builder.HasIndex(c => new { c.Name, c.Email })
               .IsUnique(true);

        // Relationship One to One
        builder.HasOne(c => c.Driver)
               .WithOne(c => c.User)
               .HasForeignKey<Driver>(c => c.UserId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.NoAction);

        // Relationship One to Many
        builder.HasOne(c => c.UserRole)
               .WithMany(c => c.ListUser)
               .HasForeignKey(c => c.UserRoleId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);
    }
}