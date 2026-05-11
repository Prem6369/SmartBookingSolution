using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotificationService.Models;

public class NotificationLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? Recipient { get; set; }

    public string? EventType { get; set; }

    public string? Message { get; set; }

    public string? Status { get; set; }

    public DateTime SentAt { get; set; }
}