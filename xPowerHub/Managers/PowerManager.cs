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
        readonly IDataStore _dataStore;

        public PowerManager(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<PowerUsage[]> GetWeekdayAvgAsync()
        {
            return (await _dataStore.GetPowerUsageWeekdayAvgAsync()).ToArray();
        }
        public async Task<PowerUsage[]> GetDayHourlyAvgAsync(DateTime date)
        {
            return (await _dataStore.GetPowerUsageHourlyAvgAsync(date)).ToArray();
        }

        public async Task<bool> AddUsageAsync(PowerUsage powerUsage)
        {
            var hourPowerUsage = await _dataStore.GetPowerUsage(powerUsage.Taken);
            if (hourPowerUsage == null)
                return await _dataStore.AddPowerStatementAsync(
                    new PowerUsage() { 
                        Taken = DateTime.ParseExact(powerUsage.Taken.ToString("yyyy-MM-dd HH"), "yyyy-MM-dd HH", CultureInfo.InvariantCulture),
                        WattHour = powerUsage.WattHour
                    }
                );
            else
            {
                hourPowerUsage.WattHour += powerUsage.WattHour;
                return await _dataStore.UpdatePowerUsage(hourPowerUsage);
            }

        }

        public async Task<double> GetpowerUsageTodayAvgAsync()
        {
             return (await _dataStore.GetPowerUsageHourlyAvgAsync(DateTime.Now)).Sum(p => p.WattHour);
        }
    }
}
