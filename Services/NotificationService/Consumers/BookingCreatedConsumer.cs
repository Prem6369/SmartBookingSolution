using System.Text;
using System.Text.Json;
using Contracts.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NotificationService.Models;
using NotificationService.Services.Email.Interfaces;
using NotificationService.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationService.Consumers;

public class BookingCreatedConsumer : BackgroundService
{
    private readonly RabbitMqSettings _rabbitMqSettings;

    private readonly IEmailService _emailService;

    private readonly IMongoCollection<NotificationLog> _logs;

    public BookingCreatedConsumer(
        IOptions<RabbitMqSettings> rabbitMqOptions,
        IOptions<MongoDbSettings> mongoOptions,
        IEmailService emailService)
    {
        _rabbitMqSettings = rabbitMqOptions.Value;

        _emailService = emailService;

        var mongoClient = new MongoClient(
            mongoOptions.Value.ConnectionString);

        var database = mongoClient.GetDatabase(
            mongoOptions.Value.DatabaseName);

        _logs = database.GetCollection<NotificationLog>(
            mongoOptions.Value.CollectionName);
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSettings.HostName
        };

        var connection = await factory.CreateConnectionAsync();

        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: _rabbitMqSettings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();

            var jsonMessage = Encoding.UTF8.GetString(body);

            var bookingEvent =
                JsonSerializer.Deserialize<BookingCreatedEvent>(
                    jsonMessage);

            if (bookingEvent != null)
            {
                await _emailService.SendEmailAsync(
                    bookingEvent.Email,
                    "Booking Confirmed",
                    $"Hello {bookingEvent.CustomerName}, your booking for {bookingEvent.EventName} is confirmed.");

                var log = new NotificationLog
                {
                    Recipient = bookingEvent.Email,

                    EventType = "BookingCreated",

                    Message = "Booking confirmation email sent",

                    Status = "Success",

                    SentAt = DateTime.UtcNow
                };

                await _logs.InsertOneAsync(log);

                Console.WriteLine(
                    $"Notification processed for {bookingEvent.Email}");
            }
        };

        await channel.BasicConsumeAsync(
            queue: _rabbitMqSettings.QueueName,
            autoAck: true,
            consumer: consumer);
    }
}