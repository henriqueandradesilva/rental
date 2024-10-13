using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configuration;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
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
        builder.Property(c => c.Description)
               .HasColumnType("varchar(100)")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.DailyRate)
               .HasColumnType("double precision")
               .IsRequired();

        builder.Property(c => c.AdditionalRate)
               .HasColumnType("double precision")
               .IsRequired();

        builder.Property(c => c.DailyLateFee)
               .HasColumnType("double precision")
               .IsRequired();

        builder.Property(c => c.DurationInDays)
               .IsRequired();

        builder.Property(c => c.IsActive)
               .HasColumnType("boolean")
               .HasDefaultValue(true);

        // Index
        builder.HasIndex(c => new { c.Description })
               .IsUnique(true);

        // Relationship One to Many
        builder.HasOne(c => c.PlanType)
               .WithMany(c => c.ListPlan)
               .HasForeignKey(c => c.PlanTypeId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);

        // Relationship Many to One
        builder.HasMany(c => c.ListRental)
               .WithOne(c => c.Plan)
               .HasForeignKey(c => c.PlanId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}