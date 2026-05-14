using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogService.Models;

public class EventCatalog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string EventName { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Category { get; set; } = default!;

    public string OrganizerName { get; set; } = default!;

    public string VenueName { get; set; } = default!;

    public string City { get; set; } = default!;

    public string Address { get; set; } = default!;

    public DateTime EventDate { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal TicketPrice { get; set; }

    public decimal? DiscountPrice { get; set; }

    public int TotalSeats { get; set; }

    public int AvailableSeats { get; set; }

    public int BookedSeats { get; set; }

    public string BannerImageUrl { get; set; } = default!;

    public bool IsActive { get; set; }

    public bool IsCancelled { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}