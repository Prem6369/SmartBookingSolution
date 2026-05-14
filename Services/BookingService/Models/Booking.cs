using BookingService.Models.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingService.Models;

public class Booking
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? BookingNumber { get; set; }

    public string? CustomerName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }
    
    public string? EventId { get; set; }

    public string? EventName { get; set; }

    public string? EventLocation { get; set; }

    public DateTime EventDate { get; set; }

    public int TotalSeats { get; set; }

    public decimal TicketPrice { get; set; }

    public decimal TotalAmount { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public BookingStatus BookingStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}