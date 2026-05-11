using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnalyticsService.Models;

public class EventPopularity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string EventName { get; set; } = default!;

    public int BookingCount { get; set; }

    public decimal TotalRevenue { get; set; }

    public DateTime UpdatedAt { get; set; }
}