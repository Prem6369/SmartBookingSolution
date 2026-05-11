namespace BookingService.DTOs.Responses;

public class BookingResponse
{
    public string? Id { get; set; }

    public string? BookingNumber { get; set; }

    public string? CustomerName { get; set; }

    public string? EventName { get; set; }

    public decimal TotalAmount { get; set; }

    public string? BookingStatus { get; set; }
}