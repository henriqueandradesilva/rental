using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace WebApi.Extensions;

public static class LoggingExtension
{
    public static IServiceCollection AddInvalidRequestLogging(
        this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(o =>
        {
            o.InvalidModelStateResponseFactory = actionContext =>
            {
                ILogger<Program> logger = actionContext
                    .HttpContext
                    .RequestServices
                    .GetRequiredService<ILogger<Program>>();

                List<string> errors = actionContext.ModelState
                    .Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                string jsonModelState = JsonSerializer.Serialize(errors);

                ValidationProblemDetails problemDetails = new ValidationProblemDetails(actionContext.ModelState);

                return new BadRequestObjectResult(problemDetails);
            };
        });

        return services;
    }
}