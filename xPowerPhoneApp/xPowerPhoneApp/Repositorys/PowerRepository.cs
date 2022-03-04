using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    }
}
