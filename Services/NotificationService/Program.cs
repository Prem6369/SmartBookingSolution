using NotificationService.Consumers;
using NotificationService.Services.Email;
using NotificationService.Services.Email.Interfaces;
using NotificationService.Settings;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqSettings>(
    builder.Configuration.GetSection("RabbitMqSettings"));

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IEmailService, EmailService>();

builder.Services.AddHostedService<BookingCreatedConsumer>();

var host = builder.Build();

host.Run();