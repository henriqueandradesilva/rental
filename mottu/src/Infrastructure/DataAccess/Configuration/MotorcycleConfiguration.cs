using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configuration;

public class MotorcycleConfiguration : IEntityTypeConfiguration<Motorcycle>
{
    public void Configure(EntityTypeBuilder<Motorcycle> builder)
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

        builder.Property(c => c.Year)
               .IsRequired();

        builder.Property(c => c.Plate)
               .HasColumnType("varchar(10)")
               .HasMaxLength(10)
               .IsRequired();

        builder.Property(c => c.IsRented)
               .HasColumnType("boolean")
               .HasDefaultValue(false);

        // Index
        builder.HasIndex(c => new { c.Plate })
               .IsUnique(true);

        // Relationship One to Many
        builder.HasOne(c => c.ModelVehicle)
               .WithMany(c => c.ListMotorcycle)
               .HasForeignKey(c => c.ModelVehicleId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);

        // Relationship Many to One
        builder.HasMany(c => c.ListRental)
               .WithOne(c => c.Motorcycle)
               .HasForeignKey(c => c.MotorcycleId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.ListMotorcycleEventCreated)
               .WithOne(c => c.Motorcycle)
               .HasForeignKey(c => c.MotorcycleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}