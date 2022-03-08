using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;

namespace xPowerPhoneApp.Repositorys.Interfaces
{
    internal interface IPowerPriceRepository
    {
        Task<PowerPrice[]> GetPowerPrice(DateTime date);
    }
}
