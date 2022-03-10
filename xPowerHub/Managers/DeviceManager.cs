using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerHub.Communicators;
using xPowerHub.DataStore;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Managers
{
    public class DeviceManager : IDeviceManager
    {
        readonly IDataStore<WizDevice> _wizDS;
        readonly IDataStore<SmartThingsDevice> _smartDS;

        public DeviceManager(IDataStore<WizDevice> wizDS, IDataStore<SmartThingsDevice> smartDS)
        {
            _wizDS = wizDS;
            _smartDS = smartDS;
        }

        public async Task ChangeStatusAsync(KnownStatusDevice device)
        {
            ISmart? smart = default;
            // is it a WiZ or a SmartThings device?
            if (!string.IsNullOrWhiteSpace(device.Ip))
            {
                smart = new WizDevice(device.Ip, device.Id, device.Name);
            }
            else if (!string.IsNullOrWhiteSpace(device.ParentId))
            {
                var parent = await _smartDS.GetAsync(device.ParentId);
                smart = new SmartThingsDevice(device.Id, device.Name, parent.Key);
            }
            // set the state to the wanted state
            smart?.SetState(device.Status);
        }

        public async Task<List<Device>> GetAllAsync()
        {
            var devices = new List<Device>();
            var wizDevices = await _wizDS.GetAllAsync();
            foreach (var wizDevice in wizDevices)
            {
                devices.Add(new Device()
                {
                    Name = string.IsNullOrWhiteSpace(wizDevice.Name) ? wizDevice.IP : wizDevice.Name,
                    Id = wizDevice.MAC,
                    Ip = wizDevice.IP
                });
            }
            var smartHubs = await _smartDS.GetAllAsync();
            foreach (var hub in smartHubs)
            {
                var smartDevices = await SmartThingsCommunicator.GetDevices(hub);

                foreach (var device in smartDevices)
                {
                    devices.Add(new Device()
                    {
                        Name = device.Name,
                        Id = device.UUID,
                        ParentId = hub.UUID
                    });
                }
            }
            return devices;
        }

        public async Task<List<KnownStatusDevice>> GetStatusAsync(List<Device> devices)
        {
            var knownDevices = new List<KnownStatusDevice>();
            foreach(Device device in devices)
            {
                ISmart smart;
                if (string.IsNullOrWhiteSpace(device.Ip))
                {
                    var parent = await _smartDS.GetAsync(device.ParentId);
                    smart = new SmartThingsDevice(device.Id, device.Name, parent.Key);
                }
                else
                {
                    smart = await _wizDS.GetAsync(device.Id);
                }
                var state = smart.GetCurrentState();
                if(state != null)
                    knownDevices.Add(new KnownStatusDevice(device) { Status = smart.GetCurrentState() ?? false });
            }
            return knownDevices;
        }

        public Device? GetNewDevice()
        {
            var device = WizDeviceCommunicator.GetNewDevice();

            if (device == null) return null;

            return new Device() { Name = device.Name, Id = device.MAC, Ip = device.IP };
        }

        public async Task AddNewDeviceAsync(Device device)
        {
            await _wizDS.SaveAsync(new WizDevice(device.Ip, device.Id, device.Name));
        }
        public async Task AddNewDeviceAsync(KeyedDevice device)
        {
            await _smartDS.SaveAsync(new SmartThingsDevice(device.Id, device.Name, device.Key));
        }

        public async Task<bool> UpdateDevice(Device device)
        {
            var dev = await _wizDS.GetAsync(device.Id);
            if (dev is not null)
            {
                var newDev = new WizDevice(device.Ip ?? dev.IP, device.Id, device.Name ?? dev.Name);
                return await _wizDS.UpdateAsync(newDev);
            }
            else
            {
                var parent = await _smartDS.GetAsync(device.ParentId);
                var smartDevice = new SmartThingsDevice(device.Id, device.Name, parent.Key);
                return await _smartDS.UpdateAsync(smartDevice);
            }
        }

        public async Task<Device[]> GetAllHubsAsync(string key)
        {
            List<Device> devices = new List<Device>();
            var hubs = await SmartThingsCommunicator.GetHubs(key);
            foreach (var hub in hubs)
            {
                devices.Add(new Device()
                {
                    Name = hub.Name,
                    Id = hub.UUID
                });
            }
            return devices.ToArray();
        }

        public async Task<double> GetAllWattageUsageAsync()
        {
            var hubs = await _smartDS.GetAllAsync();
            var k = (SmartThingsDevice hub) => SmartThingsCommunicator.GetDevices(hub).Result.Sum(x => x.GetWatt());
            return hubs.ToList().Sum(k);
        }
    }
}
