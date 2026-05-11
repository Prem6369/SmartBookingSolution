using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingService.Models;

namespace BookingService.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllAsync();

        Task<Booking?> GetByIdAsync(string id);

        Task CreateAsync(Booking booking);

        Task UpdateAsync(string id, Booking booking);

        Task DeleteAsync(string id);
    }
}