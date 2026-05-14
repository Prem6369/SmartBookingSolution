using CatalogService.DTOs.Requests;
using CatalogService.DTOs.Responses;
using CatalogService.Models;

namespace CatalogService.Services.Interfaces;

public interface IEventCatalogService
{
    Task<List<EventCatalog>> GetAllAsync();

    Task<EventCatalog?> GetByIdAsync(string id);

    Task<EventCatalog> CreateAsync(EventCatalog catalog);

    Task<bool> UpdateAsync(string id,EventCatalog catalog);

    Task<bool> DeleteAsync(string id);
    Task<AvailabilityResponse>CheckAvailabilityAsync(CheckAvailabilityRequest request);
}