using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnalyticsService.Models;

public class BookingEventLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string BookingId { get; set; } = default!;

    public string CustomerName { get; set; } = default!;

    public string EventName { get; set; } = default!;

    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; }
}