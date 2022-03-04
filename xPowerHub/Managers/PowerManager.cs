using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerHub.DataStore;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Managers
{
    public class PowerManager : IPowerManager
    {
        readonly IDataStore _dataStore;

        public PowerManager(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<PowerStatement[]> GetWeekdayAvgAsync()
        {
            return (await _dataStore.GetPowerStatementWeekdayAvgAsync()).ToArray();
        }
        public async Task<PowerStatement[]> GetDayHourlyAvgAsync(DateTime date)
        {
            return (await _dataStore.GetPowerStatementHourlyAvgAsync(date)).ToArray();
        }
    }
}
