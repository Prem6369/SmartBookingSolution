using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogService.DTOs.Requests;

public class CheckAvailabilityRequest
{
    [BsonRepresentation(BsonType.ObjectId)]

    public string EventId { get; set; } = default!;

    public int RequestedSeats { get; set; }
}