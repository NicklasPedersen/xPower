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
        readonly IDataStore dataStore;

        public DeviceManager()
        {
            var dal = new DAL(@".\database.db");
            dal.AddTables();
            dataStore = dal;
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
                var getParentTask = dataStore.GetSmartAsync(device.ParentId);
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
            var getDbDevicesTask =  dataStore.GetAllDevices();
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
                    var getParentTask = dataStore.GetSmartAsync(device.ParentId);
                    getParentTask.Wait();
                    var parent = getParentTask.Result;
                    var smartDevice = new SmartThingsDevice(device.Id, device.Name, parent.Key);
                    knownDevices.Add(new KnownStatusDevice(device) { Status = (bool)smartDevice.GetCurrentState() });
                }
                else
                {
                    var smartDevice = new WizDevice(device.Name, device.Id);
                    knownDevices.Add(new KnownStatusDevice(device) { Status = (bool)smartDevice.GetCurrentState() });
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
                dataStore.AddSmartAsync(new SmartThingsDevice(keyedDevice.Id, keyedDevice.Name, keyedDevice.Key));
            }
            else
            {
                dataStore.AddWizAsync(new WizDevice(device.Name, device.Id)).Wait();
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
