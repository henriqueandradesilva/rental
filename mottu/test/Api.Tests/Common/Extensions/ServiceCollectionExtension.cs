using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests.Common.Extensions;

public static class ServiceCollectionExtension
{
    public static void Remove<T>(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
        if (descriptor is not null) services.Remove(descriptor);
    }
}
