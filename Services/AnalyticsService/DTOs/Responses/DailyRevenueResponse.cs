namespace AnalyticsService.DTOs.Responses;

public class DailyRevenueResponse
{
    public DateTime Date { get; set; }

    public int TotalBookings { get; set; }

    public decimal TotalRevenue { get; set; }
}