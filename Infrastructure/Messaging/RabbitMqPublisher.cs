using System.Text;
using System.Text.Json;
using Messaging.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;


namespace Messaging.RabbitMQ;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly RabbitMqSettings _settings;

    public RabbitMqPublisher(IOptions<RabbitMqSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task PublishAsync<T>(T message)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName
        };

        using var connection =await factory.CreateConnectionAsync();

        using var channel =await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: _settings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var jsonMessage =JsonSerializer.Serialize(message);

        var body =Encoding.UTF8.GetBytes(jsonMessage);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: _settings.QueueName,
            body: body);

        Console.WriteLine($"RabbitMQ Message Published: {jsonMessage}");
        await Task.CompletedTask;
    }
}