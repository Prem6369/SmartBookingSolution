using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnalyticsService.Models;

public class DailyBookingAnalytics
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public DateTime Date { get; set; }

    public int TotalBookings { get; set; }

    public decimal TotalRevenue { get; set; }
}