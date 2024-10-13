using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace CrossCutting.Extensions.Api;

public static class DataProtectionExtension
{
    public static IServiceCollection AddCustomDataProtection(
        this IServiceCollection services)
    {
        services.AddDataProtection()
            .SetApplicationName("WebApi")
            .PersistKeysToFileSystem(new DirectoryInfo(@"./"));

        return services;
    }
}