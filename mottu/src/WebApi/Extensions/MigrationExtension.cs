using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace WebApi.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(
        this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<MottuDbContext>();

        var connection =
            context.Database.GetConnectionString();

        if (context.Database.GetPendingMigrations().Any())
            context.Database.Migrate();
    }
}
