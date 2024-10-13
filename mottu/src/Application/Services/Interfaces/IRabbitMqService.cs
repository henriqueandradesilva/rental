namespace Application.Services.Interfaces;

public interface IRabbitMqService
{
    void SendMessage(
        object message,
        string queueName);
}