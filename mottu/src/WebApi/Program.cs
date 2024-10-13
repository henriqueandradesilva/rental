using Application.Services;
using Application.Services.Interfaces;
using CrossCutting.Conventations;
using CrossCutting.Extensions.Api;
using CrossCutting.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using RabbitMQ.Client;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System;
using System.Globalization;
using WebApi.Extensions;
using WebApi.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddAuthenticationCustom(builder.Configuration);
builder.Services.AddFeatureFlags(builder.Configuration);
builder.Services.AddHealthChecks(builder.Configuration);
builder.Services.AddPostgreSql(builder.Configuration, null);
builder.Services.AddControllers(o =>
{
    o.Conventions.Add(new ControllerDocumentationConventation());
});
builder.Services.AddInvalidRequestLogging();
builder.Services.AddVersioning();
builder.Services.AddSwagger();
builder.Services.AddUseCases();
builder.Services.AddCustomControllers();
builder.Services.AddCustomCors();
builder.Services.AddProxy();
builder.Services.AddCustomDataProtection();
builder.Services.AddMemoryCache();
builder.Services.Configure<FirebaseStorageSettings>(builder.Configuration.GetSection("FirebaseSettings"));
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));
builder.Services.AddScoped<IFireBaseService, FireBaseService>();
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
    .WriteTo.Console()
    .WriteTo.File(
        formatter: new RenderedCompactJsonFormatter(),
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information
    ));

AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);

var app = builder.Build();

var cultureInfo = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.ApplyMigrations();
}
else
{
    app.UseExceptionHandler("/api/V1/CustomError")
       .UseHsts();
}

app.UseProxy(builder.Configuration)
   .UseHealthChecks()
   .UseCustomCors()
   .UseCustomHttpMetrics()
   .UseRouting()
   .UseVersionedSwagger(app.Services.GetRequiredService<IApiVersionDescriptionProvider>(), builder.Configuration, app.Environment)
   .UseAuthentication()
   .UseAuthorization()
   .UseEndpoints(endpoints =>
   {
       endpoints.MapControllers();
       endpoints.MapMetrics();
   });

app.Run();

public partial class Program { }
