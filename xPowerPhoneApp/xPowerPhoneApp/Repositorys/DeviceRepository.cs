using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;

namespace xPowerPhoneApp.Repositorys
{
    internal class DeviceRepository : IDeviceRepository
    {
        public Task<List<ControlDevice>> GetAllDevices()
        {
            throw new NotImplementedException();
        }

        public Task<List<ControlDevice>> GetStatusOnDevices(List<ControlDevice> controllDevices)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStatus(ControlDevice device)
        {
            throw new NotImplementedException();
        }
    }
}
