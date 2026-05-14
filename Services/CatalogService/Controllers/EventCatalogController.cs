using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogService.Models;
using CatalogService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CatalogService.DTOs.Requests;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventCatalogController : ControllerBase
    {
        private readonly IEventCatalogService _catalogService;

        public EventCatalogController(IEventCatalogService catalogService)
        {
            _catalogService = catalogService;
        }

          // GET ALL EVENTS
        [HttpGet]
        public async Task<IActionResult>GetAll()
        {
            var events =await _catalogService.GetAllAsync();

            return Ok(events);
        }

        // GET EVENT BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult>GetById(string id)
        {
            var eventCatalog =await _catalogService.GetByIdAsync(id);

            if (eventCatalog == null)
                return NotFound();

            return Ok(eventCatalog);
        }

        // CREATE EVENT
        [HttpPost]
        public async Task<IActionResult>Create(EventCatalog catalog)
        {
            var createdEvent =await _catalogService.CreateAsync(catalog);

            return Ok(createdEvent);
        }

        // UPDATE EVENT
        [HttpPut("{id}")]
        public async Task<IActionResult>Update(string id,EventCatalog catalog)
        {
            var updated = await _catalogService.UpdateAsync(id, catalog);

            if (!updated)
                return NotFound();

            return Ok("Event updated successfully");
        }

        // DELETE EVENT
        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(string id)
        {
            var deleted = await _catalogService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return Ok("Event deleted successfully");
        }

        [HttpPost("check-availability")]
        public async Task<IActionResult>CheckAvailability(CheckAvailabilityRequest request)
        {
            var result =await _catalogService.CheckAvailabilityAsync(request);

            return Ok(result);
        }
    }
}