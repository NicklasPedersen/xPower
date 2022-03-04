using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowerController : ControllerBase
    {
        private readonly IPowerManager _powerManager;
        public PowerController(IPowerManager powerManager)
        {
            _powerManager = powerManager;
        }

        [HttpGet("WeekdayAvg")]
        public async Task<PowerStatement[]> GetWeekdayAvgAsync()
        {
            return await _powerManager.GetWeekdayAvgAsync();
        }

        [HttpGet("DayHourlyAvg/{date}")]
        public async Task<PowerStatement[]> GetDayHourlyAvgAsync(string date)
        {
            return await _powerManager.GetDayHourlyAvgAsync(DateTime.Parse(date));
        }

    }
}
