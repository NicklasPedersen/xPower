using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    }
}
