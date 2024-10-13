using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.Extensions.Api;

public static class ReverseProxyExtension
{
    public static IServiceCollection AddProxy(
        this IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        return services;
    }

    public static IApplicationBuilder UseProxy(
        this IApplicationBuilder app,
        IConfiguration configuration)
    {
        string basePath = configuration["ASPNETCORE_BASEPATH"];

        if (!string.IsNullOrEmpty(basePath))
        {
            app.Use(async (context, next) =>
            {
                context.Request.PathBase = basePath;
                await next.Invoke()
                    .ConfigureAwait(false);
            });
        }

        app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });

        return app;
    }
}