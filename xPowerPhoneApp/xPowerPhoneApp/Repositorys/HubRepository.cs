using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;
using xPowerPhoneApp.Repositorys.Interfaces;
using xPowerPhoneApp.Repositorys.Shared;

namespace xPowerPhoneApp.Repositorys
{
    internal class HubRepository : IHubRepository
    {
        private readonly SharedHttpClient _httpClient;

        public HubRepository(SharedHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddHub(KeyedDevice device)
        {

            return await _httpClient.Post("hub/Add", device);

        }

        public async Task<Device[]> GetHubs(string key)
        {
            List<Device> devices = null;

            devices = (await _httpClient.Post<Device[]>("hub/GetHubs", key)).ToList();

            return devices.ToArray();
        }
    }
}
