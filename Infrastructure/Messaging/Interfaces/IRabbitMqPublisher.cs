namespace Messaging.Interfaces;

public interface IRabbitMqPublisher
{
    Task PublishAsync<T>(T message);
}