using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingService.DTOs.Requests;
using BookingService.DTOs.Responses;
using BookingService.Models;

namespace BookingService.Services.Interfaces
{
    public interface IBookingServiceManager     
    {
    Task<List<Booking>> GetAllAsync();

    Task<Booking?> GetByIdAsync(string id); 

    Task<BookingResponse> CreateAsync(CreateBookingRequest request);

    Task<bool> UpdateAsync(string id, UpdateBookingRequest request);

    Task<bool> DeleteAsync(string id);
    }
}