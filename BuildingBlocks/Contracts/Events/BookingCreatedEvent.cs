namespace Contracts.Events;

public class BookingCreatedEvent
{
    public string BookingId { get; set; } = default!;

    public string CustomerName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string EventName { get; set; } = default!;

    public decimal TotalAmount { get; set; }
}