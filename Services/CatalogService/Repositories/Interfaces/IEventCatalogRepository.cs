using CatalogService.Models;

namespace CatalogService.Repositories.Interfaces;

public interface IEventCatalogRepository
{
    Task<List<EventCatalog>> GetAllAsync();

    Task<EventCatalog?> GetByIdAsync(string id);

    Task CreateAsync(EventCatalog catalog);

    Task UpdateAsync(string id, EventCatalog catalog);

    Task DeleteAsync(string id);
}