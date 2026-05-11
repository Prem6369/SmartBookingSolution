using BookingService.DTOs.Requests;
using BookingService.DTOs.Responses;
using BookingService.Models;
using BookingService.Repositories.Interfaces;
using BookingService.Services.Cache.Interfaces;
using BookingService.Services.Interfaces;
using Contracts.Events;
using Messaging.Interfaces;

namespace BookingService.Services;

public class BookingServiceManager : IBookingServiceManager
{
    private readonly IBookingRepository _repository;
    private readonly IRedisCacheService _redisCacheService;
    private readonly IRabbitMqPublisher _publisher;
    public BookingServiceManager(IBookingRepository repository, IRedisCacheService redisCacheService, IRabbitMqPublisher publisher)
    {
        _repository = repository;
        _redisCacheService = redisCacheService;
        _publisher = publisher;
    }

    public async Task<List<Booking>> GetAllAsync()
{
    const string cacheKey = "bookings:all";

    var cachedBookings =
        await _redisCacheService.GetAsync<List<Booking>>(cacheKey);

    if (cachedBookings != null)
    {
        Console.WriteLine("Data from Redis");

        return cachedBookings;
    }

    Console.WriteLine("Data from MongoDB");

    var bookings = await _repository.GetAllAsync();

    await _redisCacheService.SetAsync(
        cacheKey,
        bookings,
        TimeSpan.FromMinutes(5));

    return bookings;
}
 
    public async Task<Booking?> GetByIdAsync(string id)
    {
        string cacheKey = $"booking:{id}";
        var booking = await _redisCacheService.GetAsync<Booking>(cacheKey);
        if (booking != null)
            return booking;

        booking = await _repository.GetByIdAsync(id);
        if (booking != null)
        {
            await _redisCacheService.SetAsync(cacheKey, booking, TimeSpan.FromMinutes(10));
        }

        return booking;
    }

    public async Task<BookingResponse> CreateAsync(CreateBookingRequest request)
    {
        var booking = new Booking
        {
            BookingNumber = $"BOOK-{Guid.NewGuid().ToString()[..8]}",

            CustomerName = request.CustomerName,

            Email = request.Email,

            PhoneNumber = request.PhoneNumber,

            EventName = request.EventName,

            EventLocation = request.EventLocation,

            EventDate = request.EventDate,

            TotalSeats = request.TotalSeats,

            TicketPrice = request.TicketPrice,

            TotalAmount = request.TotalSeats * request.TicketPrice,

            BookingStatus = Models.Enums.BookingStatus.Confirmed,

            PaymentStatus = Models.Enums.PaymentStatus.Paid,

            CreatedAt = DateTime.UtcNow,

            UpdatedAt = DateTime.UtcNow
        };

        await _repository.CreateAsync(booking);
        await _redisCacheService.RemoveAsync("bookings:all");  
        var bookingCreatedEvent = new BookingCreatedEvent
        {
            BookingId = booking.Id!,

            CustomerName = booking.CustomerName!,

            Email = booking.Email!,

            EventName = booking.EventName!,

            TotalAmount = booking.TotalAmount
        };

        await _publisher.PublishAsync(bookingCreatedEvent);

        return new BookingResponse
        {
            Id = booking.Id,

            BookingNumber = booking.BookingNumber,

            CustomerName = booking.CustomerName,

            EventName = booking.EventName,

            TotalAmount = booking.TotalAmount,

            BookingStatus = booking.BookingStatus.ToString()
        };
    }

    public async Task<bool> UpdateAsync(string id, UpdateBookingRequest request)
    {
        var existingBooking = await _repository.GetByIdAsync(id);

        if (existingBooking == null)
            return false;

        existingBooking.CustomerName = request.CustomerName;

        existingBooking.PhoneNumber = request.PhoneNumber;

        existingBooking.TotalSeats = request.TotalSeats;

        existingBooking.TotalAmount =
            existingBooking.TotalSeats * existingBooking.TicketPrice;

        existingBooking.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(id, existingBooking);
        await _redisCacheService.RemoveAsync($"booking:{id}");
        await _redisCacheService.RemoveAsync("bookings:all");

        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var existingBooking = await _repository.GetByIdAsync(id);

        if (existingBooking == null)
            return false;

        await _repository.DeleteAsync(id);
        await _redisCacheService.RemoveAsync($"booking:{id}");
        await _redisCacheService.RemoveAsync("bookings:all");

        return true;
    }
}