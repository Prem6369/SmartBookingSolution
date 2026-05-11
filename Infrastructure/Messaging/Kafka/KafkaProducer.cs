using System.Text.Json;
using Confluent.Kafka;
using Messaging.Kafka.Interfaces;

namespace Messaging.Kafka;

public class KafkaProducer : IKafkaProducer
{
    private readonly ProducerConfig _config;

    public KafkaProducer()
    {
        _config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };
    }

    public async Task ProduceAsync<T>(string topic,T message)
    {
        using var producer =new ProducerBuilder<Null, string>(_config).Build();

        var jsonMessage =JsonSerializer.Serialize(message);

        await producer.ProduceAsync(
            topic,
            new Message<Null, string>
            {
                Value = jsonMessage
            });

        Console.WriteLine($"Kafka Message Published: {jsonMessage}");
    }
}