using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerHub.DataStore;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Managers
{
    public class PowerManager : IPowerManager
    {
        readonly IDataStorePower _powerDS;

        public PowerManager(IDataStorePower powerDS)
        {
            _powerDS = powerDS;
        }

        public async Task<PowerUsage[]> GetWeekdayAvgAsync()
        {
            return (await _powerDS.GetPowerUsageWeekdayAvgAsync()).ToArray();
        }
        public async Task<PowerUsage[]> GetDayHourlyAvgAsync(DateTime date)
        {
            return (await _powerDS.GetPowerUsageHourlyAvgAsync(date)).ToArray();
        }

        public async Task<bool> AddUsageAsync(PowerUsage powerUsage)
        {
            var hourPowerUsage = await _powerDS.GetAsync(powerUsage.Taken);
            if (hourPowerUsage == null)
                return await _powerDS.SaveAsync(
                    new PowerUsage() { 
                        Taken = DateTime.ParseExact(powerUsage.Taken.ToString("yyyy-MM-dd HH"), "yyyy-MM-dd HH", CultureInfo.InvariantCulture),
                        WattHour = powerUsage.WattHour
                    }
                );
            else
            {
                hourPowerUsage.WattHour += powerUsage.WattHour;
                return await _powerDS.UpdateAsync(hourPowerUsage);
            }

        }

        public async Task<double> GetpowerUsageTodayAvgAsync()
        {
             return (await _powerDS.GetPowerUsageHourlyAvgAsync(DateTime.Now)).Sum(p => p.WattHour);
        }
    }
}
