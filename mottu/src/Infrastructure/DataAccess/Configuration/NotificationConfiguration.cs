using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configuration;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
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
               .IsRequired();

        // Index
        builder.HasIndex(c => new { c.OrderId })
               .IsUnique(false);

        // Relationship One to Many
        builder.HasOne(c => c.Order)
               .WithMany(c => c.ListNotification)
               .HasForeignKey(c => c.OrderId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);

        // Relationship Many to One
        builder.HasMany(c => c.ListDriverNotificated)
               .WithOne(c => c.Notification)
               .HasForeignKey(c => c.NotificationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}