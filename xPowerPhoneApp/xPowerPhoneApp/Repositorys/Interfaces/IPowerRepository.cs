using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;

namespace xPowerPhoneApp.Repositorys.Interfaces
{
    public interface IPowerRepository
    {
        /// <summary>
        /// Gets the current power level
        /// </summary>
        /// <returns>The current power level</returns>
        Task<double> GetPowerLevel();

        /// <summary>
        /// Gets the powerusage for current day
        /// </summary>
        /// <returns>The usage</returns>
        Task<double> GetTodaysPowerUsage();

        /// <summary>
        /// Gets The powerusage for the given day
        /// </summary>
        /// <param name="dateTime">The date it should get</param>
        /// <returns>The hourly usage</returns>
        Task<PowerUsage[]> GetDaysPowerUsage(DateTime dateTime);

        /// <summary>
        /// Gets the week avg power usage
        /// </summary>
        /// <returns>The usages for each weekday</returns>
        Task<PowerUsage[]> GetWeeklyAvg();
    }
}
