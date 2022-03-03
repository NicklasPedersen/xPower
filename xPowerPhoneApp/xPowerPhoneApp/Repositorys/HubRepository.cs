﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;
using xPowerPhoneApp.Repositorys.Shared;

namespace xPowerPhoneApp.Repositorys
{
    internal class HubRepository : IHubRepository
    {
        public async Task<bool> AddHub(KeyedDevice device)
        {

            return await SharedHttpClient.Instants.Post("hub/Add", device);

        }

        public async Task<Device[]> GetHubs(string key)
        {
            List<Device> devices = null;

            devices = (await SharedHttpClient.Instants.Post<Device[]>("hub/GetHubs", key)).ToList();

            return devices.ToArray();
        }
    }
}