﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            dataStore = new DAL(@"..\xPowerHub\data\database.db");
        }

        public void ChangeStatus(KnownStatusDevice device)
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

        public List<Device> GetAll()
        {
            var devices = new List<Device>();
            var dbDevices =  dataStore.GetAllDevices();
            dbDevices.Wait();
            foreach (var smartDevice in dbDevices.Result)
            {
                if(smartDevice is WizDevice wizDevice)
                    devices.Add(new Device() { Name = wizDevice.IP, Id = wizDevice.MAC });
            }
            return devices;
        }

        public List<KnownStatusDevice> GetStatus(List<Device> devices)
        {
            var knownDevices = new List<KnownStatusDevice>();
            foreach(Device device in devices)
            {
                var smartDevice = new WizDevice(device.Name, device.Id);
                knownDevices.Add(new KnownStatusDevice(device) { Status = (bool)smartDevice.GetCurrentState() });
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
            dataStore.AddWizAsync(new WizDevice(device.Name, device.Id)).Wait(); 
        }
    }
}
