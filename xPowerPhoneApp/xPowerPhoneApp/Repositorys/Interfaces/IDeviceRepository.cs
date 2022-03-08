using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;

namespace xPowerPhoneApp.Repositorys.Interfaces
{
    internal interface IDeviceRepository
    {
        /// <summary>
        /// Gets all the devices connected to the system
        /// </summary>
        /// <returns>List of connected devices</returns>
        Task<List<ControlDevice>> GetAllDevices();

        /// <summary>
        /// Gets the status of the devices
        /// </summary>
        /// <param name="controllDevices">The devices that will get statuses</param>
        /// <returns>A list of devices with there status</returns>
        Task<List<ControlDevice>> GetStatusOnDevices(List<ControlDevice> controllDevices);
        Task<bool> UpdateStatus(ControlDevice device);
        Task<bool> UpdateName(Device device);
    }
}
