using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;
using xPowerPhoneApp.Repositorys.Interfaces;

namespace xPowerPhoneApp.Repositorys.Mocks
{
    internal class PowerRepositoryMock : IPowerRepository
    {
        private double _power;

        public async Task<double> GetPowerLevel()
        {
            Random random = new Random();

            var power = (random.NextDouble() - 0.5) * 10;

            await Task.Delay(random.Next(0, 125));
            if (_power + power > 0)
                _power += power;
            return _power;
        }

        public async Task<double> GetTodaysPowerUsage()
        {
            await Task.Delay(1);

            return _power*0.4;
        }

        public async Task<PowerUsage[]> GetWeeklyAvg()
        {
            await Task.Delay(1);
            Random random = new Random();

            List<PowerUsage> powerUsageList = new List<PowerUsage>();

            for (int i = 0; i < 7; i++)
            {
                powerUsageList.Add(new PowerUsage() { WattHour = random.NextDouble() * 1000, Taken = DateTime.Now.AddDays(i) });
            }

            return powerUsageList.ToArray();
        }

        public async Task<PowerUsage[]> GetDaysPowerUsage(DateTime dateTime)
        {
            await Task.Delay(1);
            Random random = new Random();

            List<PowerUsage> powerUsageList = new List<PowerUsage>();

            for (int i = 0; i < 24; i++)
            {
                powerUsageList.Add(new PowerUsage() { WattHour = random.NextDouble() * 100, Taken = DateTime.Now.AddHours(DateTime.Now.Hour * -1).AddHours(i) });
            }

            return powerUsageList.ToArray();
        }
    }
}
