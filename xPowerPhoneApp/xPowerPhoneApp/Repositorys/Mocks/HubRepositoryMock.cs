using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;
using xPowerPhoneApp.Repositorys.Interfaces;

namespace xPowerPhoneApp.Repositorys.Mocks
{
    internal class HubRepositoryMock : IHubRepository
    {
        public async Task<bool> AddHub(KeyedDevice device)
        {
            Random random = new Random();
            await Task.Delay(random.Next(100, 250));
            return true;
        }

        public async Task<Device[]> GetHubs(string key)
        {
            Random random = new Random();
            await Task.Delay(random.Next(100, 250));
            
            var devices = new Device[random.Next(1,4)];
            for (int i = 0; i < devices.Length; i++)
            {
                devices[i] = new Device() { Id = "testDevice" + i, Name = "testhub number " + (i + 1) };
            }

            return devices;
        }
    }
}
