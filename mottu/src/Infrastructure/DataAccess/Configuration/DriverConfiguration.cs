using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configuration;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
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
        builder.Property(c => c.Identifier)
               .HasColumnType("varchar(100)")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.Name)
               .HasColumnType("varchar(150)")
               .IsRequired();

        builder.Property(c => c.Cnpj)
               .HasColumnType("varchar(14)")
               .HasMaxLength(14)
               .IsRequired();

        builder.Property(c => c.Cnh)
               .HasColumnType("varchar(50)")
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(c => c.CnhImageUrl)
               .HasColumnType("text")
               .IsRequired(false);

        builder.Property(c => c.Type)
               .HasColumnType("varchar(50)")
               .HasConversion<string>()
               .IsRequired();

        builder.Property(c => c.Delivering)
               .HasColumnType("boolean")
               .HasDefaultValue(false);

        builder.Property(c => c.DateOfBirth)
               .HasColumnType("date")
               .IsRequired();

        // Index
        builder.HasIndex(c => new { c.Identifier, c.Cnpj, c.Cnh })
               .IsUnique(true);

        // Relationship One to One
        builder.HasOne(c => c.User)
               .WithOne(c => c.Driver)
               .HasForeignKey<Driver>(c => c.UserId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.NoAction);

        // Relationship Many to One
        builder.HasMany(c => c.ListDriverNotificated)
               .WithOne(c => c.Driver)
               .HasForeignKey(c => c.DriverId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.ListAccepted)
               .WithOne(c => c.Driver)
               .HasForeignKey(c => c.DriverId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(c => c.ListDelivered)
               .WithOne(c => c.Driver)
               .HasForeignKey(c => c.DriverId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(c => c.ListRental)
               .WithOne(c => c.Driver)
               .HasForeignKey(c => c.DriverId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}