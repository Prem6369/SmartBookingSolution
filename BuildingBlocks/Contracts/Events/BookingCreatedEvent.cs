namespace Contracts.Events;

public class BookingCreatedEvent
{
    public string BookingId { get; set; } = default!;

    public string EventId { get; set; } = default!;

    public string EventName { get; set; } = default!;

    public string CustomerName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public int TotalSeats { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime BookingDate { get; set; }
}