using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerHub.DataStore;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Managers.Test
{
    public class PowerManagerTest : IPowerManager
    {
        public PowerManagerTest()
        {
        }

        public async Task<PowerUsage[]> GetWeekdayAvgAsync()
        {
            var random = new Random();
            await Task.Delay(random.Next(100, 301));
            var powerUsages = new List<PowerUsage>();
            for (int i = 0; i < 7; i++)
            {
                powerUsages.Add(new PowerUsage() { Taken = DateTime.Now.AddDays(i), WattHour = random.NextDouble()*1000 });
            }
            return powerUsages.ToArray();
        }

        public async Task<PowerUsage[]> GetDayHourlyAvgAsync(DateTime date)
        {
            var random = new Random();
            await Task.Delay(random.Next(100, 301));
            var powerUsages = new List<PowerUsage>();
            for (int i = 0; i < 24; i++)
            {
                powerUsages.Add(new PowerUsage() { Taken = DateTime.Parse(date.ToString("yyyy-MM-dd")).AddHours(i), WattHour = random.NextDouble() * 100 });
            }
            return powerUsages.ToArray();
        }

        public async Task<bool> AddUsageAsync(PowerUsage powerUsage)
        {
            throw new NotImplementedException();
        }

        public async Task<double> GetpowerUsageTodayAvgAsync()
        {
            var random = new Random();
            await Task.Delay(random.Next(100, 301));
            return random.NextDouble()*100;
        }
    }
}
