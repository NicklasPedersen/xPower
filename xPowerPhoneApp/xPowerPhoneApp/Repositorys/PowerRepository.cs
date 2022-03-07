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
        public async Task<double> GetPowerLevel()
        {
            return await SharedHttpClient.Instants.Get<double>("Device/GetCurrentWattage");
        }

        public async Task<double> GetTodaysPowerUsage()
        {
            return await SharedHttpClient.Instants.Get<double>("Power/Today");
        }

        public async Task<PowerUsage[]> GetWeeklyAvg()
        {
            return await SharedHttpClient.Instants.Get<PowerUsage[]>("Power/WeekdayAvg");
        }

        public async Task<PowerUsage[]> GetDaysPowerUsage(DateTime dateTime)
        {
            return await SharedHttpClient.Instants.Get<PowerUsage[]>("Power/DayHourlyAvg/"+ dateTime.ToString("dd-MM-yyyy"));
        }

    }
}
