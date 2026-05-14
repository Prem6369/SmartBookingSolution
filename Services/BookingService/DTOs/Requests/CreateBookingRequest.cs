namespace BookingService.DTOs.Requests;

public class CreateBookingRequest
{
    public string? CustomerName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }
    public string? EventId { get; set; }
    public string? EventName { get; set; }

    public string? EventLocation { get; set; }

    public DateTime EventDate { get; set; }

    public int TotalSeats { get; set; }

    public decimal TicketPrice { get; set; }
}