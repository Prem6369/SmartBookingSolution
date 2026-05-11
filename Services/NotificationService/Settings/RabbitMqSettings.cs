namespace NotificationService.Settings;

public class RabbitMqSettings
{
    public string HostName { get; set; } = default!;

    public string QueueName { get; set; } = default!;
}