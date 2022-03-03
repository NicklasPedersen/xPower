using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;

namespace xPowerPhoneApp.Repositorys
{
    internal class SmartUnitRepositoryMock : ISmartUnitRepository
    {
        private readonly string[] _names = new string[]
        {
            "Wiz smart plug",
            "nice plug",
            "I was here",
            "Wiz smart Light",
            "hue light",
            "hue plug"
        };

        public async Task<bool> AddDevice(AddDevice device)
        {
            Random random = new Random();
            await Task.Delay(random.Next(0, 1000));

            return true;
        }

        public async Task<List<AddDevice>> GetDevices()
        {
            await Task.Delay(1);
            Random random = new Random();

            var devices = new List<AddDevice>();
            var amount = random.Next(0, 10);

            for (int i = 0; i < amount; i++)
            {
                devices.Add(new AddDevice()
                {
                    Name = _names[random.Next(0,_names.Length)], 
                    Id = BitConverter.ToString(BitConverter.GetBytes(random.Next(0,10000)))
                });
            }
            return devices;
        }

        public async Task<List<AddDevice>> GetNewDevices(List<AddDevice> knownDevices)
        {
            await Task.Delay(300);

            Random random = new Random();

            var devices = new List<AddDevice>();
            var amount = random.Next(0, 2);

            for (int i = 0; i < amount; i++)
            {
                devices.Add(new AddDevice() { Name = _names[random.Next(0, _names.Length)], Id = BitConverter.ToString(BitConverter.GetBytes(random.Next(0, 10000))) });
            }
            return devices;
        }
    }
}
