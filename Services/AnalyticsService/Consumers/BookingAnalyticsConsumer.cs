using System.Text.Json;
using AnalyticsService.Models;
using AnalyticsService.Settings;
using Confluent.Kafka;
using Contracts.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AnalyticsService.Consumers;

public class BookingAnalyticsConsumer : BackgroundService
{
    private readonly KafkaSettings _kafkaSettings;

    private readonly IMongoCollection<BookingEventLog> _eventLogs;

    private readonly IMongoCollection<DailyBookingAnalytics> _dailyAnalytics;
    private readonly IMongoCollection<EventPopularity> _eventPopularity;

    public BookingAnalyticsConsumer(IOptions<KafkaSettings> kafkaOptions,IOptions<MongoDbSettings> mongoOptions)
    {
        _kafkaSettings = kafkaOptions.Value;

        var mongoClient = new MongoClient(mongoOptions.Value.ConnectionString);

        var database = mongoClient.GetDatabase(mongoOptions.Value.DatabaseName);

        _eventLogs =database.GetCollection<BookingEventLog>("BookingEventLogs");

        _dailyAnalytics =database.GetCollection<DailyBookingAnalytics>("DailyBookingAnalytics");
        _eventPopularity = database.GetCollection<EventPopularity>("EventPopularity");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers =_kafkaSettings.BootstrapServers,

            GroupId =_kafkaSettings.GroupId,

            AutoOffsetReset =AutoOffsetReset.Earliest
        };

        using var consumer =new ConsumerBuilder<Ignore, string>(config).Build();

        consumer.Subscribe(_kafkaSettings.TopicName);

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(stoppingToken);

            var bookingEvent =JsonSerializer.Deserialize<BookingCreatedEvent>(consumeResult.Message.Value);

            if (bookingEvent != null)
            {
                // Store raw event log

                var eventLog = new BookingEventLog
                {
                    BookingId = bookingEvent.BookingId,
                    CustomerName =bookingEvent.CustomerName,
                    EventName =bookingEvent.EventName,
                    TotalAmount =bookingEvent.TotalAmount,
                    CreatedAt = DateTime.UtcNow
                };

                await _eventLogs.InsertOneAsync(eventLog);

                // Update daily analytics

                var popularity =
                    await _eventPopularity.Find(x => x.EventName == bookingEvent.EventName).FirstOrDefaultAsync();

                if (popularity == null)
                {
                    popularity = new EventPopularity
                    {
                        EventName = bookingEvent.EventName,
                        BookingCount = 1,
                        TotalRevenue = bookingEvent.TotalAmount,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _eventPopularity.InsertOneAsync(popularity);
                }
                else
                {
                    popularity.BookingCount += 1;
                    popularity.TotalRevenue +=bookingEvent.TotalAmount;
                    popularity.UpdatedAt = DateTime.UtcNow;
                    await _eventPopularity.ReplaceOneAsync(x => x.Id == popularity.Id,popularity);
                }

                var today = DateTime.UtcNow.Date;

                var analytics =await _dailyAnalytics.Find(x => x.Date == today).FirstOrDefaultAsync();

                if (analytics == null)
                {
                    analytics =
                        new DailyBookingAnalytics
                        {
                            Date = today,
                            TotalBookings = 1,
                            TotalRevenue =bookingEvent.TotalAmount
                        };

                    await _dailyAnalytics.InsertOneAsync(analytics);
                }
                else
                {
                    analytics.TotalBookings += 1;
                    analytics.TotalRevenue +=bookingEvent.TotalAmount;
                    await _dailyAnalytics.ReplaceOneAsync(x => x.Id == analytics.Id,analytics);
                }

                Console.WriteLine($"Analytics Processed: {bookingEvent.BookingId}");
            }
        }
    }
}