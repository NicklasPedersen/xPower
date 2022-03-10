using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;
using xPowerPhoneApp.Repositorys.Interfaces;
using xPowerPhoneApp.Repositorys.Shared;

namespace xPowerPhoneApp.Repositorys
{
    internal class DeviceRepository : IDeviceRepository
    {
        private readonly SharedHttpClient _httpClient;

        public DeviceRepository(SharedHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ControlDevice>> GetAllDevices()
        {
            List<ControlDevice> devices = null;

            devices = (await _httpClient.Get<ControlDevice[]>("Device/GetAll")).ToList();

            return devices;
        }

        public async Task<List<ControlDevice>> GetStatusOnDevices(List<ControlDevice> controllDevices)
        {
            List<ControlDevice> devices = null;

            devices = (await _httpClient.Post<ControlDevice[]>("Device/GetStatus", controllDevices.ToArray())).ToList();

            for (int i = 0; i < devices.Count; i++)
            {
                devices[i].IsStatusKnown = true;
            }

            return devices;
        }

        public async Task<bool> UpdateStatus(ControlDevice device)
        {
            return await _httpClient.Post("Device/ChangeStatus", device);
        }

        public async Task<bool> UpdateName(Device device)
        {
            return await _httpClient.Post("Device/ChangeName", device);
        }
    }
}
