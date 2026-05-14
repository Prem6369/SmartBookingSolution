using CatalogService.Models;
using CatalogService.Repositories.Interfaces;
using CatalogService.Services.Interfaces;
using CatalogService.DTOs.Requests;
using CatalogService.DTOs.Responses;
namespace CatalogService.Services;

public class EventCatalogManager: IEventCatalogService
{
    private readonly IEventCatalogRepository _repository;

    public EventCatalogManager(IEventCatalogRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<EventCatalog>>GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<EventCatalog?>GetByIdAsync(string id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<EventCatalog>CreateAsync(EventCatalog catalog)
    {
        catalog.AvailableSeats =catalog.TotalSeats;

        catalog.BookedSeats = 0;

        catalog.IsActive = true;

        catalog.IsCancelled = false;

        catalog.CreatedAt = DateTime.UtcNow;

        catalog.UpdatedAt = DateTime.UtcNow;

        await _repository.CreateAsync(catalog);

        return catalog;
    }

    public async Task<bool>UpdateAsync(string id,EventCatalog catalog)
    {
        var existingCatalog =await _repository.GetByIdAsync(id);

        if (existingCatalog == null)
            return false;

        catalog.Id = existingCatalog.Id;

        catalog.CreatedAt =existingCatalog.CreatedAt;

        catalog.UpdatedAt =DateTime.UtcNow;

        await _repository.UpdateAsync(id,catalog);

        return true;
    }

    public async Task<bool>DeleteAsync(string id)
    {
        var existingCatalog =await _repository.GetByIdAsync(id);

        if (existingCatalog == null)return false;

        await _repository.DeleteAsync(id);

        return true;
    }
    public async Task<AvailabilityResponse>CheckAvailabilityAsync(CheckAvailabilityRequest request)
    {
    var eventCatalog =await _repository.GetByIdAsync(request.EventId);

    if (eventCatalog == null)
    {
        return new AvailabilityResponse
        {
            Available = false,

            AvailableSeats = 0,

            Message = "Event not found"
        };
    }

    var available =eventCatalog.AvailableSeats>= request.RequestedSeats;

    return new AvailabilityResponse
    {
        Available = available,

        AvailableSeats =eventCatalog.AvailableSeats,

        Message = available
            ? "Seats available"
            : "Insufficient seats available"
    };
}
}