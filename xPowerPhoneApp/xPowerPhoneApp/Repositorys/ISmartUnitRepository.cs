using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;

namespace xPowerPhoneApp.Repositorys
{
    internal interface ISmartUnitRepository
    {
        Task<List<AddDevice>> GetDevices();
        Task<bool> AddDevice(AddDevice device);
        Task<List<AddDevice>> GetNewDevices(List<AddDevice> knownDevices);
    }
}
