namespace AnalyticsService.Settings;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = default!;

    public string TopicName { get; set; } = default!;

    public string GroupId { get; set; } = default!;
}