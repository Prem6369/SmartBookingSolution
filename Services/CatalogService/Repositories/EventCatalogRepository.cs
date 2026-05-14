using CatalogService.Models;
using CatalogService.Repositories.Interfaces;
using CatalogService.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CatalogService.Repositories;

public class EventCatalogRepository: IEventCatalogRepository
{
    private readonly IMongoCollection<EventCatalog>_catalogCollection;

    public EventCatalogRepository(IOptions<MongoDbSettings> mongoOptions)
    {
        var mongoClient = new MongoClient(mongoOptions.Value.ConnectionString);

        var database = mongoClient.GetDatabase(mongoOptions.Value.DatabaseName);

        _catalogCollection =database.GetCollection<EventCatalog>(mongoOptions.Value.CollectionName);
    }

    public async Task<List<EventCatalog>>GetAllAsync()
    {
        return await _catalogCollection.Find(_ => true).ToListAsync();
    }

    public async Task<EventCatalog?>GetByIdAsync(string id)
    {
        return await _catalogCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(EventCatalog catalog)
    {
        await _catalogCollection.InsertOneAsync(catalog);
    }

    public async Task UpdateAsync(string id,EventCatalog catalog)
    {
        await _catalogCollection.ReplaceOneAsync(x => x.Id == id, catalog);
    }

    public async Task DeleteAsync(string id)
    {
        await _catalogCollection.DeleteOneAsync(x => x.Id == id);
    }
}