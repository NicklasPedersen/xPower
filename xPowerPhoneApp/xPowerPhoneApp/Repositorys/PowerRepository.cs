using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;
using xPowerPhoneApp.Repositorys.Interfaces;
using xPowerPhoneApp.Repositorys.Shared;

namespace xPowerPhoneApp.Repositorys
{
    internal class PowerRepository : IPowerRepository
    {
        private readonly SharedHttpClient _httpClient;

        public PowerRepository(SharedHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<double> GetPowerLevel()
        {
            return await _httpClient.Get<double>("Power/GetCurrentWattage");
        }

        public async Task<double> GetTodaysPowerUsage()
        {
            return await _httpClient.Get<double>("Power/Today");
        }

        public async Task<PowerUsage[]> GetWeeklyAvg()
        {
            return await _httpClient.Get<PowerUsage[]>("Power/WeekdayAvg");
        }

        public async Task<PowerUsage[]> GetDaysPowerUsage(DateTime dateTime)
        {
            return await _httpClient.Get<PowerUsage[]>("Power/DayHourlyAvg/"+ dateTime.ToString("dd-MM-yyyy"));
        }

    }
}
