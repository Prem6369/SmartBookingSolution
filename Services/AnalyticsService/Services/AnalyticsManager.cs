using AnalyticsService.DTOs.Responses;
using AnalyticsService.Models;
using AnalyticsService.Services.Interfaces;
using AnalyticsService.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AnalyticsService.Services;

public class AnalyticsManager : IAnalyticsService
{
    private readonly IMongoCollection<EventPopularity> _eventPopularity;

    private readonly IMongoCollection<DailyBookingAnalytics> _dailyAnalytics;

    public AnalyticsManager(IOptions<MongoDbSettings> mongoOptions)
    {
        var mongoClient = new MongoClient(mongoOptions.Value.ConnectionString);

        var database = mongoClient.GetDatabase(mongoOptions.Value.DatabaseName);

        _eventPopularity =database.GetCollection<EventPopularity>("EventPopularity");

        _dailyAnalytics =database.GetCollection<DailyBookingAnalytics>("DailyBookingAnalytics");
    }

    public async Task<List<PopularEventResponse>> GetPopularEventsAsync()
    {
        var events = await _eventPopularity.Find(_ => true).SortByDescending(x => x.BookingCount).ToListAsync();

        return events.Select(x => new PopularEventResponse
        {
            EventName = x.EventName,
            BookingCount = x.BookingCount,
            TotalRevenue = x.TotalRevenue
        }).ToList();
    }

    public async Task<DashboardResponse> GetDashboardAsync()
    {
        var analytics =await _dailyAnalytics.Find(_ => true).ToListAsync();

        var totalBookings =analytics.Sum(x => x.TotalBookings);

        var totalRevenue =analytics.Sum(x => x.TotalRevenue);

        var topEvent =await _eventPopularity.Find(_ => true).SortByDescending(x => x.BookingCount).FirstOrDefaultAsync();

        return new DashboardResponse
        {
            TotalBookings = totalBookings,
            TotalRevenue = totalRevenue,
            TopEvent = topEvent?.EventName ?? "N/A"
        };
    }
    public async Task<List<DailyRevenueResponse>> GetDailyRevenueAsync()
    {
        var analytics =await _dailyAnalytics.Find(_ => true).SortBy(x => x.Date).ToListAsync();

        return analytics.Select(x =>
            new DailyRevenueResponse
            {
                Date = x.Date,
                TotalBookings = x.TotalBookings,
                TotalRevenue = x.TotalRevenue
            }).ToList();
    }
}