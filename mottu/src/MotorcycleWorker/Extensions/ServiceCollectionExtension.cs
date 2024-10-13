using Application.UseCases.V1.MotorcycleEventCreated.PostMotorcycleEventCreated;
using Application.UseCases.V1.MotorcycleEventCreated.PostMotorcycleEventCreated.Interfaces;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Repositories;
using Infrastructure.DataAccess;
using Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleWorker.Worker;
using MotorcycleWorker.Worker.Interfaces;

namespace MotorcycleWorker.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreSql(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MottuDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IMotorcycleEventCreatedRepository, MotorcycleEventCreatedRepository>();

        return services;
    }

    public static IServiceCollection AddUseCases(
        this IServiceCollection services)
    {
        services.AddScoped<IPostMotorcycleEventCreatedUseCase, PostMotorcycleEventCreatedUseCase>();
        services.Decorate<IPostMotorcycleEventCreatedUseCase, PostMotorcycleEventCreatedValidationUseCase>();

        services.AddScoped<NotificationHelper, NotificationHelper>();

        return services;
    }

    public static IServiceCollection AddWorker(
        this IServiceCollection services)
    {
        services.AddScoped<IMotorcycleEventWorker, MotorcycleEventWorker>();

        return services;
    }
}
