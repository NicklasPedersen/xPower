using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;

namespace xPowerPhoneApp.Repositorys.Interfaces
{
    internal interface ISmartUnitRepository
    {
        /// <summary>
        /// Gets all the devices that can be connected
        /// </summary>
        /// <returns>List of devics</returns>
        Task<List<AddDevice>> GetDevices();

        /// <summary>
        /// Saves the devices data
        /// </summary>
        /// <param name="device">The device that will be saved</param>
        /// <returns>If it got saved</returns>
        Task<bool> AddDevice(AddDevice device);

        /// <summary>
        /// Gets the new devices that is not in the given list
        /// </summary>
        /// <param name="knownDevices">List of already known devices</param>
        /// <returns>A list of new devices</returns>
        Task<List<AddDevice>> GetNewDevices(List<AddDevice> knownDevices);
    }
}
