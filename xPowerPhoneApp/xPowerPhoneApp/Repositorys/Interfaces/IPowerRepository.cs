using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xPowerPhoneApp.Repositorys.Interfaces
{
    internal interface IPowerRepository
    {
        /// <summary>
        /// Gets the current power level
        /// </summary>
        /// <returns>The current power level</returns>
        Task<double> GetPowerLevel();
    }
}
