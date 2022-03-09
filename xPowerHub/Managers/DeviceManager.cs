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
            if (!string.IsNullOrWhiteSpace(device.Ip))
            {
                var smartDevice = new WizDevice(device.Name, device.Id);
                if (device.Status)
                {
                    smartDevice.TurnOn();
                }
                else
                {
                    smartDevice.TurnOff();
                }
            }
            else if (!string.IsNullOrWhiteSpace(device.ParentId))
            {
                var parent = await _smartDS.GetAsync(device.ParentId);
                var smartDevice = new SmartThingsDevice(device.Id, device.Name, parent.Key);
                if (device.Status)
                {
                    smartDevice.TurnOn();
                }
                else
                {
                    smartDevice.TurnOff();
                }
            }
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
            foreach (var smartDevice in smartHubs)
            {
                var smartDevices = await SmartThingsCommunicator.GetDevices(smartDevice);

                foreach (var device in smartDevices)
                {
                    devices.Add(new Device()
                    {
                        Name = device.Name,
                        Id = device.UUID,
                        ParentId = smartDevice.UUID
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
                if (string.IsNullOrWhiteSpace(device.Ip))
                {
                    var parent = await _smartDS.GetAsync(device.ParentId);
                    var smartDevice = new SmartThingsDevice(device.Id, device.Name, parent.Key);
                    knownDevices.Add(new KnownStatusDevice(device) { Status = (bool)smartDevice.GetCurrentState() });
                }
                else
                {
                    var wizDevice = await _wizDS.GetAsync(device.Id);
                    var status = wizDevice?.GetCurrentState();
                    // if status is null we did not get a response from the device
                    // TODO: handle unresponsive devices
                    knownDevices.Add(new KnownStatusDevice(device) { Status = (bool)status });
                }
            }
            return knownDevices;
        }

        public Device? GetNewDevice()
        {
            var device = WizDeviceCommunicator.GetNewDevice();

            if (device == null) return null;

            return new Device() { Name = device.IP, Id = device.MAC, Ip = device.IP };
        }

        public async Task AddNewDeviceAsync(Device device)
        {
            if (device is KeyedDevice keyedDevice)
            {
                await _smartDS.SaveAsync(new SmartThingsDevice(keyedDevice.Id, keyedDevice.Name, keyedDevice.Key));
            }
            else
            {
                await _wizDS.SaveAsync(new WizDevice(device.Ip, device.Id, device.Ip));
            }
        }

        public async Task<bool> ChangeDevice(Device device)
        {
            if (string.IsNullOrWhiteSpace(device.Ip))
            {
                var parent = await _smartDS.GetAsync(device.ParentId);
                var smartDevice = new SmartThingsDevice(device.Id, device.Name, parent.Key);
                return await _smartDS.UpdateAsync(smartDevice);
            }
            else
            {
                var smartDevice = new WizDevice(device.Ip, device.Id, device.Name);
                return await _wizDS.UpdateAsync(smartDevice);
            }
        }

        public async Task<Device[]> GetAllHubsAsync(string key)
        {
            List<Device> devices = new List<Device>();
            var smartDevices = await SmartThingsCommunicator.GetHubs(key);
            foreach (var device in smartDevices)
            {
                devices.Add(new Device()
                {
                    Name = device.Name,
                    Id = device.UUID
                });
            }
            return devices.ToArray();
        }

        public async Task<double> GetAllWattageUsageAsync()
        {
            var l = await _smartDS.GetAllAsync();
            var k = (SmartThingsDevice s) => SmartThingsCommunicator.GetDevices(s).Result.Sum(x => x.GetWatt());
            return l.ToList().Sum(k);
        }
    }
}
