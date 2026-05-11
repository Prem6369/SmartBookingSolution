using BookingService.Models;
using BookingService.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookingService.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly IMongoCollection<Booking> _bookings;

    public BookingRepository(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

        _bookings = mongoDatabase.GetCollection<Booking>(
            settings.Value.BookingCollectionName);
    }

    public async Task<List<Booking>> GetAllAsync()
    {
        return await _bookings.Find(_ => true).ToListAsync();
    }

    public async Task<Booking?> GetByIdAsync(string id)
    {
        return await _bookings.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Booking booking)
    {
        await _bookings.InsertOneAsync(booking);
    }

    public async Task UpdateAsync(string id, Booking booking)
    {
        await _bookings.ReplaceOneAsync(x => x.Id == id, booking);
    }

    public async Task DeleteAsync(string id)
    {
        await _bookings.DeleteOneAsync(x => x.Id == id);
    }
}

