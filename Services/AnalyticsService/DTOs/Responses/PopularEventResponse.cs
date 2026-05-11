namespace AnalyticsService.DTOs.Responses;

public class PopularEventResponse
{
    public string EventName { get; set; } = default!;

    public int BookingCount { get; set; }

    public decimal TotalRevenue { get; set; }
}