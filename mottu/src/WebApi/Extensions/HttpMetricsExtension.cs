using Microsoft.AspNetCore.Builder;
using Prometheus;

namespace WebApi.Extensions;

public static class HttpMetricsExtension
{
    public static IApplicationBuilder UseCustomHttpMetrics(
        this IApplicationBuilder appBuilder) =>
        appBuilder
            .UseMetricServer()
            .UseHttpMetrics(options =>
            {
                options.RequestDuration.Enabled = true;
                options.InProgress.Enabled = true;
                options.RequestCount.Enabled = true;
            });
}