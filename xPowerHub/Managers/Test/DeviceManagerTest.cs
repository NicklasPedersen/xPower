using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerHub.Communicators;
using xPowerHub.DataStore;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Managers.Test
{
    public class DeviceManagerTest : IDeviceManager
    {
        public DeviceManagerTest()
        {
        }

        public async Task ChangeStatusAsync(KnownStatusDevice device)
        {
            var random = new Random();
            await Task.Delay(random.Next(100,301));
        }

        public async Task<List<Device>> GetAllAsync()
        {
            var random = new Random();
            await Task.Delay(random.Next(100, 301));
            var devices = new List<Device>();
            devices.Add(new Device() { Id = "testMAC", Ip = "12.34.56.78", Name = "wiz Test" });
            devices.Add(new Device() { Id = "testuidd", Name = "hub Test" });
            return devices;
        }

        public async Task<List<KnownStatusDevice>> GetStatusAsync(List<Device> devices)
        {
            var random = new Random();
            await Task.Delay(random.Next(100, 301));
            var knownDevices = new List<KnownStatusDevice>();
            foreach (var device in devices)
            {
                knownDevices.Add(new KnownStatusDevice(device) { Status = random.Next(0,2) == 1});
            }
            return knownDevices;
        }

        public Device? GetNewDevice()
        {
            return new Device() { Id="testMAC", Ip="12.34.56.78", Name="wiz Test"};
        }

        public async Task AddNewDeviceAsync(Device device)
        {
            var random = new Random();
            await Task.Delay(random.Next(100, 301));
        }

        public async Task AddNewDeviceAsync(KeyedDevice device)
        {
            var random = new Random();
            await Task.Delay(random.Next(100, 301));
        }

        public async Task<bool> UpdateDevice(Device device)
        {
            var random = new Random();
            await Task.Delay(random.Next(100, 301));
            return true;
        }

        public async Task<Device[]> GetAllHubsAsync(string key)
        {
            var random = new Random();
            await Task.Delay(random.Next(100, 301));
            var devices = new List<Device>();
            devices.Add(new Device() { Id = "testuidd", Name = "hub Test" });
            return devices.ToArray();
        }

        public async Task<double> GetAllWattageUsageAsync()
        {
            var random = new Random();
            await Task.Delay(random.Next(100, 301));
            return random.NextDouble()*100;
        }
    }
}
