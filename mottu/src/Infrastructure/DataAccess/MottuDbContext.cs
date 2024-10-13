using Infrastructure.DataAccess.Seeds;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Infrastructure.DataAccess;

public sealed class MottuDbContext : DbContext
{
    public MottuDbContext(
        DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
            throw new ArgumentNullException(nameof(modelBuilder));

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MottuDbContext).Assembly);

        SeedData.Seed(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        AuditLog();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void AuditLog()
    {
        var entries = ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
                Log.Debug($"Entity: {entry.Entity.GetType().Name}, Date Updated after set: {entry.CurrentValues["DateUpdated"]}");

            if (entry.State == EntityState.Added)
                Log.Debug($"Entity: {entry.Entity.GetType().Name}, Date Created set: {entry.CurrentValues["DateCreated"]}");

            if (entry.State == EntityState.Deleted)
            {
                var entityType = entry.Entity.GetType().Name;
                var entityId = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "Id")?.CurrentValue;

                if (entityId != null)
                    Log.Debug($"Entity: {entityType}, Id: {entityId}");
            }
        }
    }
}