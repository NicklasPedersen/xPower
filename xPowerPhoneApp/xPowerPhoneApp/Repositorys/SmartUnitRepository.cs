using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;
using xPowerPhoneApp.Repositorys.Interfaces;
using xPowerPhoneApp.Repositorys.Shared;

namespace xPowerPhoneApp.Repositorys
{
    internal class SmartUnitRepository : ISmartUnitRepository
    {
        private readonly SharedHttpClient _httpClient;

        public SmartUnitRepository(SharedHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddDevice(AddDevice device)
        {
            return await _httpClient.Post("Unit/Add", device);
        }

        public async Task<List<AddDevice>> GetDevices()
        {
            var device = await _httpClient.Get<AddDevice>("Unit/GetNew");
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
