using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configuration;

public class OrderDeliveredConfiguration : IEntityTypeConfiguration<OrderDelivered>
{
    public void Configure(EntityTypeBuilder<OrderDelivered> builder)
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
        builder.Property(c => c.Date)
               .HasColumnType("timestamp with time zone")
               .IsRequired(true);

        // Index
        builder.HasIndex(c => new { c.DriverId, c.OrderId })
               .IsUnique(true);

        // Relationship One to One
        builder.HasOne(c => c.Order)
               .WithOne(c => c.Delivered)
               .HasForeignKey<OrderDelivered>(c => c.OrderId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);

        // Relationship One to Many
        builder.HasOne(c => c.Driver)
               .WithMany(c => c.ListDelivered)
               .HasForeignKey(c => c.DriverId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);
    }
}