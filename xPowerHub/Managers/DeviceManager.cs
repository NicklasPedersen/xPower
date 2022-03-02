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
        readonly IDataStore _dataStore;

        public DeviceManager(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public void ChangeStatus(KnownStatusDevice device)
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
            else if(!string.IsNullOrWhiteSpace(device.ParentId))
            {
                var getParentTask = _dataStore.GetSmartAsync(device.ParentId);
                getParentTask.Wait();
                var parent = getParentTask.Result;
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

        public List<Device> GetAll()
        {
            var devices = new List<Device>();
            var getDbDevicesTask = _dataStore.GetAllDevices();
            getDbDevicesTask.Wait();
            var dbDevices = getDbDevicesTask.Result;
            foreach (var smartDevice in dbDevices)
            {
                if (smartDevice is WizDevice wizDevice)
                {
                    devices.Add(new Device()
                    {
                        Name = string.IsNullOrWhiteSpace(wizDevice.Name) ? wizDevice.IP : wizDevice.Name,
                        Id = wizDevice.MAC,
                        Ip = wizDevice.IP
                    });
                }
                else if (smartDevice is SmartThingsDevice thingsDevice)
                {
                    var getDevicesTask = SmartThingsCommunicator.GetDevices(thingsDevice);
                    getDevicesTask.Wait();
                    var smartDevices = getDevicesTask.Result;

                    foreach (var device in smartDevices)
                    {
                        devices.Add(new Device()
                        {
                            Name = device.Name,
                            Id = device.UUID,
                            ParentId = thingsDevice.UUID
                        });
                    }
                }
            }
            return devices;
        }

        public List<KnownStatusDevice> GetStatus(List<Device> devices)
        {
            var knownDevices = new List<KnownStatusDevice>();
            foreach(Device device in devices)
            {
                if (string.IsNullOrWhiteSpace(device.Ip))
                {
                    var getParentTask = _dataStore.GetSmartAsync(device.ParentId);
                    getParentTask.Wait();
                    var parent = getParentTask.Result;
                    var smartDevice = new SmartThingsDevice(device.Id, device.Name, parent.Key);
                    knownDevices.Add(new KnownStatusDevice(device) { Status = (bool)smartDevice.GetCurrentState() });
                }
                else
                {                
                    var smartDevice = new WizDevice(device.Name, device.Id);
                    var status = smartDevice.GetCurrentState();
                    // if status is null we did not get a response from the device
                    // TODO: handle unresponsive devices
                    knownDevices.Add(new KnownStatusDevice(device) { Status = (bool)status });
                }
            }
            return knownDevices;
        }

        public Device GetNewDevice()
        {
            var device = WizDeviceCommunicator.GetNewDevice();

            if(device == null) return null;

            return new Device() { Name = device.IP, Id = device.MAC };
        }

        public void AddNewDevice(Device device)
        {
            if(device is KeyedDevice keyedDevice)
            {
                _dataStore.AddSmartAsync(new SmartThingsDevice(keyedDevice.Id, keyedDevice.Name, keyedDevice.Key));
            }
            else
            {
                _dataStore.AddWizAsync(new WizDevice(device.Name, device.Id)).Wait();
            }
        }

        public Device[] GetAllHubs(string key)
        {
            List<Device> devices = new List<Device>();
            var getHubsTask = SmartThingsCommunicator.GetHubs(key);
            getHubsTask.Wait();
            var smartDevices = getHubsTask.Result;
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
    }
}
