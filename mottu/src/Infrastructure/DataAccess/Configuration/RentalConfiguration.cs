using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configuration;

public class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
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
        builder.Property(c => c.AllocatePeriod)
               .IsRequired();

        builder.Property(c => c.TotalAmount)
               .HasColumnType("double precision")
               .IsRequired();

        builder.Property(c => c.StartDate)
               .HasColumnType("timestamp with time zone")
               .IsRequired();

        builder.Property(c => c.EndDate)
               .HasColumnType("timestamp with time zone")
               .IsRequired();

        builder.Property(c => c.ExpectedEndDate)
               .HasColumnType("timestamp with time zone")
               .IsRequired();

        builder.Property(c => c.Status)
               .HasColumnType("varchar(50)")
               .HasConversion<string>()
               .IsRequired();

        // Index
        builder.HasIndex(e => new { e.MotorcycleId, e.DriverId, e.PlanId })
               .IsUnique(false);

        // Relationship One to Many
        builder.HasOne(c => c.Motorcycle)
               .WithMany(c => c.ListRental)
               .HasForeignKey(c => c.MotorcycleId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Driver)
               .WithMany(c => c.ListRental)
               .HasForeignKey(c => c.DriverId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Plan)
               .WithMany(c => c.ListRental)
               .HasForeignKey(c => c.PlanId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);
    }
}