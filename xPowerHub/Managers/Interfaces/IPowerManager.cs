using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerHub.Models;

namespace xPowerHub.Managers.Interfaces
{
    public interface IPowerManager
    {
        Task<PowerStatement[]> GetWeekdayAvgAsync();
        Task<PowerStatement[]> GetDayHourlyAvgAsync(DateTime date);
    }
}
