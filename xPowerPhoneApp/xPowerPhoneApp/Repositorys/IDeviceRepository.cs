using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;

namespace xPowerPhoneApp.Repositorys
{
    internal interface IDeviceRepository
    {
        Task<List<ControlDevice>> GetAllDevices();
        Task<List<ControlDevice>> GetStatusOnDevices(List<ControlDevice> controllDevices);
        Task<bool> UpdateStatus(ControlDevice device);
    }
}
