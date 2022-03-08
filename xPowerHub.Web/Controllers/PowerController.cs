using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Web.Controllers
{

    /// <summary>
    /// Controlls all with the power
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PowerController : ControllerBase
    {
        private readonly IPowerManager _powerManager;
        private readonly IDeviceManager _deviceManager;
        public PowerController(IPowerManager powerManager, IDeviceManager deviceManager)
        {
            _powerManager = powerManager;
            _deviceManager = deviceManager;
        }

        /// <summary>
        /// Gets the avg power usage for each weekday
        /// </summary>
        /// <returns>The avg power usage</returns>
        [HttpGet("WeekdayAvg")]
        public async Task<PowerUsage[]> GetWeekdayAvgAsync()
        {
            return await _powerManager.GetWeekdayAvgAsync();
        }

        /// <summary>
        /// Gets the Power usage in hourly parts for that date
        /// </summary>
        /// <param name="date">The day it will return power usage for</param>
        /// <returns>Power usage in hourly parts</returns>
        [HttpGet("DayHourlyAvg/{date}")]
        public async Task<PowerUsage[]> GetDayHourlyAvgAsync(string date)
        {
            return await _powerManager.GetDayHourlyAvgAsync(DateTime.Parse(date));
        }

        /// <summary>
        /// Gets the Power usage for the day
        /// </summary>
        /// <returns>The power usage</returns>
        [HttpGet("Today")]
        public async Task<double> GetTodaysPowerUsage()
        {
            return await _powerManager.GetpowerUsageTodayAvgAsync();
        }

        /// <summary>
        /// Gets all wattages from all devices that supports it
        /// </summary>
        /// <returns>A number containing the sum of current wattage</returns>
        [HttpGet("GetCurrentWattage")]
        public async Task<double> GetCurrentWattageAsync()
        {
            return await _deviceManager.GetAllWattageUsageAsync();
        }
    }
}
