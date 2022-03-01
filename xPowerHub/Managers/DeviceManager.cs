using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Managers
{
    public class DeviceManager : IDeviceManager
    {

        readonly string file = @"..\xPowerHub\data.json";

        readonly List<WizDevice> smartDevices;

        public DeviceManager()
        {
            smartDevices = DeviceSerializer.Deserialize(File.ReadAllText(file)).ToList();
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

            foreach (var smartDevice in smartDevices)
            {
                devices.Add(new Device() { Name = smartDevice.IP, Id = smartDevice.MAC });
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
    }
}
