using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configuration;

public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
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

        // Index
        builder.HasIndex(c => new { c.Description })
               .IsUnique(true);

        // Relationship Many to One
        builder.HasMany(c => c.ListOrder)
               .WithOne(c => c.Status)
               .HasForeignKey(c => c.StatusId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}