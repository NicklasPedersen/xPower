using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;

namespace xPowerPhoneApp.Repositorys
{
    internal class DeviceRepositoryMock : IDeviceRepository
    {
        private readonly string[] _names = new string[]
        {
            "stueLys",
            "blæser",
            "Kokkenlys",
            "lille lampe kontakt",
            "computer",
            "tv",
            "oven lys"
        };

        public async Task<List<ControlDevice>> GetAllDevices()
        {
            await Task.Delay(1);
            var devices = new List<ControlDevice>();
            var random = new Random(); 
            var count = random.Next(4, 25);
            for (int i = 0; i < count; i++)
            {
                devices.Add(new ControlDevice() { Name = _names[random.Next(0,_names.Length)], Id = i.ToString() });
            }
            return devices;
        }

        public async Task<List<ControlDevice>> GetStatusOnDevices(List<ControlDevice> controllDevices)
        {
            await Task.Delay(1);
            var random = new Random();
            var devices = new List<ControlDevice>();
            foreach (var device in controllDevices)
            {
                if(random.Next(0, 5) != 0)
                {
                    device.Status = random.Next(0,2) == 1;
                    device.IsStatusKnown = true;
                }
                devices.Add(device);
            }
            return devices;
        }

        public async Task<bool> UpdateStatus(ControlDevice device)
        {
            var random = new Random();
            await Task.Delay(random.Next(0, 1000));
            return true;
        }
    }
}
