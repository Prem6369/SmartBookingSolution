using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingService.DTOs.Requests;
using BookingService.Models;
using BookingService.Repositories;
using BookingService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
    private readonly IBookingServiceManager _bookingService;

    public BookingController(IBookingServiceManager bookingService)
    {
        _bookingService = bookingService;
    }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAllAsync();

            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var booking = await _bookingService.GetByIdAsync(id);

            if (booking is null)
                return NotFound();

            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingRequest booking)
        {
            var createdBooking = await _bookingService.CreateAsync(booking);

            return Ok(createdBooking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateBookingRequest booking)
        {
            var updated = await _bookingService.UpdateAsync(id, booking);

            if (!updated)
                return NotFound();

            return Ok("Booking updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _bookingService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return Ok("Booking deleted successfully");
        }
    }
}