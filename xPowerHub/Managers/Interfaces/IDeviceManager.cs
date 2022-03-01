using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerHub.Models;

namespace xPowerHub.Managers.Interfaces
{
    public interface IDeviceManager
    {
        /// <summary>
        /// Gets a list of connected devices
        /// </summary>
        /// <returns>List of devices</returns>
        List<Device> GetAll();
        /// <summary>
        /// Gets the status of the given devices
        /// </summary>
        /// <param name="devices">The devices where there status will be search for</param>
        /// <returns>A array of KnownStatusDevice</returns>
        List<KnownStatusDevice> GetStatus(List<Device> devices);
        /// <summary>
        /// Changes the status of the given device
        /// </summary>
        /// <param name="device">The given device with the new status</param>
        void ChangeStatus(KnownStatusDevice device);
        /// <summary>
        /// Gets a new device
        /// </summary>
        /// <returns>The new device</returns>
        Device GetNewDevice();
        /// <summary>
        /// Addes the device to the system
        /// </summary>
        /// <param name="device">The new device</param>
        void AddNewDevice(Device device);
    }
}
