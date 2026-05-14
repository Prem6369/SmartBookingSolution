namespace CatalogService.DTOs.Responses;

public class AvailabilityResponse
{
    public bool Available { get; set; }

    public int AvailableSeats { get; set; }

    public string Message { get; set; } = default!;
}