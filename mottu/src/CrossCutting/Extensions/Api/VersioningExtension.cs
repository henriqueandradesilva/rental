using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.Extensions.Api;

public static class VersioningExtension
{
    public static IServiceCollection AddVersioning(
        this IServiceCollection services)
    {
        services.AddApiVersioning(
            options =>
            {
                options.ReportApiVersions = true;
            });
        services.AddVersionedApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }
}