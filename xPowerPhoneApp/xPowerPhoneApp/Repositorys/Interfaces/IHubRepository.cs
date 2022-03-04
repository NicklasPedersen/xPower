using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;

namespace xPowerPhoneApp.Repositorys.Interfaces
{
    internal interface IHubRepository
    {
        /// <summary>
        /// Gets all the hubs with the given key
        /// </summary>
        /// <param name="key">The key for the hub</param>
        /// <returns>A list of devices/hubs</returns>
        Task<Device[]> GetHubs(string key);

        /// <summary>
        /// Saves the hubs data
        /// </summary>
        /// <param name="device">The hub</param>
        /// <returns>If it got saved</returns>
        Task<bool> AddHub(KeyedDevice device);
    }
}
