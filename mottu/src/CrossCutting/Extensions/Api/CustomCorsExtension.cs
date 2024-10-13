using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.Extensions.Api;

public static class CustomCorsExtension
{
    private const string AllowsAny = "_allowsAny";

    public static IServiceCollection AddCustomCors(
        this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(AllowsAny,
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });

        return services;
    }

    public static IApplicationBuilder UseCustomCors(
        this IApplicationBuilder app)
    {
        app.UseCors(AllowsAny);

        return app;
    }
}