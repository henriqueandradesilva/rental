using Application.Services;
using Application.Services.Interfaces;
using Application.UseCases.V1.DriverNotificated.PostDriverNotificated;
using Application.UseCases.V1.DriverNotificated.PostDriverNotificated.Interfaces;
using Application.UseCases.V1.Notification.PostNotification;
using Application.UseCases.V1.Notification.PostNotification.Interfaces;
using CrossCutting.Helpers;
using CrossCutting.Interfaces;
using Domain.Repositories;
using Infrastructure.DataAccess;
using Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderWorker.Worker;
using OrderWorker.Worker.Interfaces;

namespace OrderWorker.Extensions;

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
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDriverNotificatedRepository, DriverNotificatedRepository>();
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }

    public static IServiceCollection AddUseCases(
        this IServiceCollection services)
    {
        services.AddScoped<IDriverNotificatedService, DriverNotificatedService>();

        services.AddScoped<IPostNotificationUseCase, PostNotificationUseCase>();
        services.Decorate<IPostNotificationUseCase, PostNotificationValidationUseCase>();

        services.AddScoped<IPostDriverNotificatedUseCase, PostDriverNotificatedUseCase>();
        services.Decorate<IPostDriverNotificatedUseCase, PostDriverNotificatedValidationUseCase>();

        services.AddScoped<NotificationHelper, NotificationHelper>();

        return services;
    }

    public static IServiceCollection AddWorker(
        this IServiceCollection services)
    {
        services.AddScoped<IDriverNotificatedWorker, DriverNotificatedWorker>();

        return services;
    }
}
