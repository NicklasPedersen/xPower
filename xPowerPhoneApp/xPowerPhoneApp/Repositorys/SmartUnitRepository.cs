using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;
using xPowerPhoneApp.Repositorys.Shared;

namespace xPowerPhoneApp.Repositorys
{
    internal class SmartUnitRepository : ISmartUnitRepository
    {
        public async Task<bool> AddDevice(AddDevice device)
        {
            return await SharedHttpClient.Instants.Post("Unit/Add", device);
        }

        public async Task<List<AddDevice>> GetDevices()
        {
            var device = await SharedHttpClient.Instants.Get<AddDevice>("Unit/GetNew");
            return new List<AddDevice>() { device };
        }

        public async Task<List<AddDevice>> GetNewDevices(List<AddDevice> knownDevices)
        {
            var newDevices = new List<AddDevice>();
            var devices = await this.GetDevices();
            foreach (var device in devices)
            {
                if (knownDevices.FindAll(d => d.Equals(device)).Count > 0) continue;
                newDevices.Add(device);
            }
            return newDevices;
        }
    }
}
