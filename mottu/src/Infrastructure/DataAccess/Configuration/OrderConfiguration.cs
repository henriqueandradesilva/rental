using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
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

        builder.Property(c => c.Value)
               .HasColumnType("double precision")
               .IsRequired();

        builder.Property(c => c.Date)
                .HasColumnType("timestamp with time zone")
                .IsRequired(true);

        // Index
        builder.HasIndex(c => new { c.StatusId })
               .IsUnique(false);

        // Relationship One to One
        builder.HasOne(o => o.Accepted)
               .WithOne(a => a.Order)
               .HasForeignKey<OrderAccepted>(a => a.OrderId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(o => o.Delivered)
               .WithOne(a => a.Order)
               .HasForeignKey<OrderDelivered>(a => a.OrderId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);

        // Relationship One to Many
        builder.HasOne(c => c.Status)
               .WithMany(c => c.ListOrder)
               .HasForeignKey(c => c.StatusId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);

        // Relationship Many to One
        builder.HasMany(c => c.ListNotification)
               .WithOne(c => c.Order)
               .HasForeignKey(c => c.OrderId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}