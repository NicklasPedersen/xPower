using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerHub.DataStore;
using xPowerHub.Managers.Interfaces;

namespace xPowerHub.Managers
{
    public class PowerManager : IPowerManager
    {
        readonly IDataStore _dataStore;

        public PowerManager(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }


    }
}
