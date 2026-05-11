using BookingService.Repositories;
using BookingService.Repositories.Interfaces;
using BookingService.Services;
using BookingService.Services.Cache;
using BookingService.Services.Cache.Interfaces;
using BookingService.Services.Interfaces;
using Messaging.Interfaces;
using Messaging.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddScoped<IBookingServiceManager, BookingServiceManager>();
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration["RedisSettings:ConnectionString"];
});

builder.Services.AddScoped<IRedisCacheService,RedisCacheService>();
builder.Services.AddScoped<IRabbitMqPublisher,RabbitMqPublisher>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
