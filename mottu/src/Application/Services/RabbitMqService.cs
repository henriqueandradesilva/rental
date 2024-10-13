using Application.Services.Interfaces;
using CrossCutting.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace Application.Services;

public class RabbitMqService : IRabbitMqService
{
    private readonly IOptions<RabbitMqSettings> _rabbitMqSettings;

    public RabbitMqService(
        IOptions<RabbitMqSettings> rabbitMqSettings)
    {
        _rabbitMqSettings = rabbitMqSettings;
    }

    public void SendMessage(
        object message,
        string queueName)
    {
        try
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = _rabbitMqSettings.Value.HostName,
                UserName = _rabbitMqSettings.Value.UserName,
                Password = _rabbitMqSettings.Value.Password
            };

            using var connection = connectionFactory.CreateConnection();

            using var channel = connection.CreateModel();

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.BasicPublish(exchange: string.Empty, routingKey: queueName, basicProperties: null, body: body);
        }
        catch (Exception ex)
        {

        }
    }
}