using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;

namespace xPowerPhoneApp.Repositorys
{
    internal interface IHubRepository
    {
        Task<Device[]> GetHubs(string key);
        Task<bool> AddHub(KeyedDevice device);
    }
}
