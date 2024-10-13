using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configuration;

public class MotorcycleEventCreatedConfiguration : IEntityTypeConfiguration<MotorcycleEventCreated>
{
    public void Configure(EntityTypeBuilder<MotorcycleEventCreated> builder)
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
        builder.Property(c => c.Json)
               .HasColumnType("text")
               .IsRequired();

        builder.Property(c => c.CurrentYear);

        // Relationship One to Many
        builder.HasOne(c => c.Motorcycle)
               .WithMany(c => c.ListMotorcycleEventCreated)
               .HasForeignKey(c => c.MotorcycleId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }
}