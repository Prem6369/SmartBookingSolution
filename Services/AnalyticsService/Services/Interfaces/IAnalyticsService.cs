using AnalyticsService.DTOs.Responses;

namespace AnalyticsService.Services.Interfaces;

public interface IAnalyticsService
{
    Task<List<PopularEventResponse>>GetPopularEventsAsync();

    Task<DashboardResponse>GetDashboardAsync();
    Task<List<DailyRevenueResponse>>GetDailyRevenueAsync();
}