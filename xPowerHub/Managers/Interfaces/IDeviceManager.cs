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
        Task<List<Device>> GetAllAsync();

        /// <summary>
        /// Gets the status of the given devices
        /// </summary>
        /// <param name="devices">The devices where there status will be search for</param>
        /// <returns>A array of KnownStatusDevice</returns>
        Task<List<KnownStatusDevice>> GetStatusAsync(List<Device> devices);
        
        /// <summary>
        /// Changes the status of the given device
        /// </summary>
        /// <param name="device">The given device with the new status</param>
        Task ChangeStatusAsync(KnownStatusDevice device);
        
        /// <summary>
        /// Gets a new device
        /// </summary>
        /// <returns>The new device</returns>
        Device? GetNewDevice();
        
        /// <summary>
        /// Addes the device to the system
        /// </summary>
        /// <param name="device">The new device</param>
        Task AddNewDeviceAsync(Device device);

        /// <summary>
        /// Gets all hubs using the given key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>A list of all the hubs</returns>
        Task<Device[]> GetAllHubsAsync(string key);


        /// <summary>
        /// Gets all wattages from all devices that supports it
        /// </summary>
        /// <returns>A number containing the sum of current wattage</returns>
        public Task<double> GetAllWattageUsageAsync();
    }
}
