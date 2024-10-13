using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configuration;

public class DriverNotificatedConfiguration : IEntityTypeConfiguration<DriverNotificated>
{
    public void Configure(EntityTypeBuilder<DriverNotificated> builder)
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
        builder.HasIndex(c => new { c.DriverId, c.NotificationId })
               .IsUnique(false);

        // Relationship One to Many
        builder.HasOne(c => c.Driver)
               .WithMany(c => c.ListDriverNotificated)
               .HasForeignKey(c => c.DriverId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Notification)
               .WithMany(c => c.ListDriverNotificated)
               .HasForeignKey(c => c.NotificationId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);
    }
}