using Application.Services;
using Application.UseCases.V1.MotorcycleEventCreated.PostMotorcycleEventCreated.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Settings;
using Domain.Entities;
using Microsoft.Extensions.Options;
using MotorcycleWorker.Worker.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MotorcycleWorker.Worker;

public class MotorcycleEventWorker : IMotorcycleEventWorker
{
    private readonly IPostMotorcycleEventCreatedUseCase _postMotorcycleEventCreatedUseCase;
    private readonly IOptions<RabbitMqSettings> _rabbitMqSettings;
    private IConnection _connection;
    private IModel _channel;
    private TimeSpan _checkInterval;
    private readonly NotificationHelper _notificationHelper;

    public MotorcycleEventWorker(
        NotificationHelper notificationHelper,
        IPostMotorcycleEventCreatedUseCase postMotorcycleEventCreatedUseCase,
        IOptions<RabbitMqSettings> rabbitMqSettings)
    {
        _notificationHelper = notificationHelper;
        _postMotorcycleEventCreatedUseCase = postMotorcycleEventCreatedUseCase;
        _rabbitMqSettings = rabbitMqSettings;
    }

    public async Task Init(
        CancellationToken cancellationToken)
    {
        using (var client = new HttpClient())
        {
            var rabbitMqUrl = $"http://{_rabbitMqSettings.Value.HostName}:{_rabbitMqSettings.Value.PortUrl}";

            bool isRabbitMqHealthy = false;

            while (!isRabbitMqHealthy && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var response =
                        await client.GetAsync(rabbitMqUrl, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        isRabbitMqHealthy = true;

                        Console.WriteLine("RabbitMQ está pronto para conexões.");
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("RabbitMQ não está disponível, tentando novamente...");
                }

                await Task.Delay(5000, cancellationToken);
            }
        }

        var connectionFactory = new ConnectionFactory()
        {
            HostName = _rabbitMqSettings.Value.HostName,
            UserName = _rabbitMqSettings.Value.UserName,
            Password = _rabbitMqSettings.Value.Password
        };

        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: SystemConst.MotorcycleEventCreatedQueue,
                              durable: true,
                              exclusive: false,
                              autoDelete: false);

        _checkInterval = _rabbitMqSettings.Value.CheckInterval;
    }

    public async Task Start(
        CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();

            var json = Encoding.UTF8.GetString(body);

            var motorcycle = JsonConvert.DeserializeObject<Motorcycle>(json);

            var motorcycleEventCreated = new MotorcycleEventCreated(0, motorcycle.Id, json);

            if (motorcycle.Year == 2024)
                motorcycleEventCreated.AddCurrentYear(true);

            var motorcycleEventCreatedOutputPort =
                new OutputPortService<MotorcycleEventCreated>(_notificationHelper);

            _postMotorcycleEventCreatedUseCase.SetOutputPort(motorcycleEventCreatedOutputPort);

            await _postMotorcycleEventCreatedUseCase.Execute(motorcycleEventCreated);

            Console.WriteLine($"Mensagem recebida: {json}");
        };

        _channel.BasicConsume(queue: SystemConst.MotorcycleEventCreatedQueue, autoAck: true, consumer: consumer);

        Console.WriteLine($"Esperando por mensagens na fila '{SystemConst.MotorcycleEventCreatedQueue}'...");

        while (!cancellationToken.IsCancellationRequested)
            await Task.Delay(_checkInterval, cancellationToken);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
