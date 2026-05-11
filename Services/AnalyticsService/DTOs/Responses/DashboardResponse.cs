namespace AnalyticsService.DTOs.Responses;

public class DashboardResponse
{
    public int TotalBookings { get; set; }

    public decimal TotalRevenue { get; set; }

    public string TopEvent { get; set; } = default!;
}