using Application.Services;
using Application.UseCases.V1.DriverNotificated.PostDriverNotificated.Interfaces;
using Application.UseCases.V1.Notification.PostNotification.Interfaces;
using CrossCutting.Const;
using CrossCutting.Helpers;
using CrossCutting.Settings;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderWorker.Worker.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderWorker.Worker;

public class DriverNotificatedWorker : IDriverNotificatedWorker
{
    private readonly IDriverRepository _driverRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IPostNotificationUseCase _postNotificationUseCase;
    private readonly IPostDriverNotificatedUseCase _postDriverNotificatedUseCase;
    private readonly IOptions<RabbitMqSettings> _rabbitMqSettings;
    private IConnection _connection;
    private IModel _channel;
    private TimeSpan _checkInterval;
    private readonly NotificationHelper _notificationHelper;

    public DriverNotificatedWorker(
        NotificationHelper notificationHelper,
        IDriverRepository driverRepository,
        IOrderRepository orderRepository,
        IPostNotificationUseCase postNotificationUseCase,
        IPostDriverNotificatedUseCase postDriverNotificatedUseCase,
        IOptions<RabbitMqSettings> rabbitMqSettings)
    {
        _driverRepository = driverRepository;
        _orderRepository = orderRepository;
        _notificationHelper = notificationHelper;
        _postNotificationUseCase = postNotificationUseCase;
        _postDriverNotificatedUseCase = postDriverNotificatedUseCase;
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

        _channel.QueueDeclare(queue: SystemConst.OrderEventCreatedQueue,
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

            var order = JsonConvert.DeserializeObject<Order>(json);

            var orderExist =
            _orderRepository?.Where(c => c.Id == order.Id)
                            ?.FirstOrDefault();

            if (orderExist != null)
            {
                var listDriver =
                await _driverRepository?.Where(c => c.User.IsActive &&
                                                    c.User.UserRole.Id == SystemConst.UserRoleDriverIdDefault &&
                                                    !c.Delivering)
                                       ?.ToListAsync();

                if (listDriver != null &&
                    listDriver.Any())
                {
                    var notification = new Notification(0, order.Id, DateTime.UtcNow);

                    var notificationOutputPort =
                        new OutputPortService<Notification>(_notificationHelper);

                    _postNotificationUseCase.SetOutputPort(notificationOutputPort);

                    await _postNotificationUseCase.Execute(notification);

                    foreach (var driver in listDriver)
                    {
                        var driverNotificated = new DriverNotificated(0, driver.Id, notification.Id, DateTime.UtcNow);

                        var driverNotificatedOutputPort =
                            new OutputPortService<DriverNotificated>(_notificationHelper);

                        _postDriverNotificatedUseCase.SetOutputPort(driverNotificatedOutputPort);

                        await _postDriverNotificatedUseCase.Execute(driverNotificated);
                    }
                }
            }

            Console.WriteLine($"Mensagem recebida: {json}");
        };

        _channel.BasicConsume(queue: SystemConst.OrderEventCreatedQueue, autoAck: true, consumer: consumer);

        Console.WriteLine($"Esperando por mensagens na fila '{SystemConst.OrderEventCreatedQueue}'...");

        while (!cancellationToken.IsCancellationRequested)
            await Task.Delay(_checkInterval, cancellationToken);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
