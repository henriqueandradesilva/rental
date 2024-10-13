using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace WebApi.Extensions.Swagger;

public sealed class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private const string UriString = "https://mottu-api.app";

    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(
        IApiVersionDescriptionProvider provider)
        => _provider = provider;

    public void Configure(
        SwaggerGenOptions options)
    {
        foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
    }

    private static OpenApiInfo CreateInfoForApiVersion(
        ApiVersionDescription description)
    {
        OpenApiInfo info = new OpenApiInfo
        {
            Title = "WebApi",
            Version = description.ApiVersion.ToString(),
            Description = ".",
            Contact = new OpenApiContact { Name = "Mottu", Email = "contato@mottu.app" },
            TermsOfService = new Uri(UriString)
        };

        if (description.IsDeprecated)
            info.Description += " This API version has been deprecated.";

        return info;
    }
}