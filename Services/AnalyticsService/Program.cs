using AnalyticsService.Consumers;
using AnalyticsService.Services;
using AnalyticsService.Services.Interfaces;
using AnalyticsService.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaSettings>(
    builder.Configuration.GetSection("KafkaSettings"));

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddHostedService<
    BookingAnalyticsConsumer>();

builder.Services.AddScoped<IAnalyticsService,AnalyticsManager>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapControllers();

app.Run();