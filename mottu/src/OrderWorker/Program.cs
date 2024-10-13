using CrossCutting.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderWorker.Extensions;
using OrderWorker.Worker.Interfaces;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(
        string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var worker = services.GetRequiredService<IDriverNotificatedWorker>();

            var cancellationToken = new CancellationTokenSource().Token;

            await worker.Init(cancellationToken);

            await worker.Start(cancellationToken);
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            var configuration = context.Configuration;
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));
            services.AddPostgreSql(configuration);
            services.AddUseCases();
            services.AddWorker();
        });
}