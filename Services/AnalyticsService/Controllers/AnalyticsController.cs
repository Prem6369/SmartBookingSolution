using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnalyticsService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController( IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("popular-events")]
        public async Task<IActionResult>GetPopularEvents()
        {
            var result =await _analyticsService.GetPopularEventsAsync();
            return Ok(result);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult>GetDashboard()
        {
            var result =await _analyticsService.GetDashboardAsync();
            return Ok(result);
        }
        [HttpGet("daily-revenue")]
        public async Task<IActionResult>GetDailyRevenue()
        {
            var result =await _analyticsService.GetDailyRevenueAsync();
            return Ok(result);
        }
    }
}