namespace BookingService.DTOs.Requests;

public class UpdateBookingRequest
{
    public string? CustomerName { get; set; }

    public string? PhoneNumber { get; set; }

    public int TotalSeats { get; set; }
}