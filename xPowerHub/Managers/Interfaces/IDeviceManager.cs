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
        List<Device> GetAll();
        List<KnownStatusDevice> GetStatus(List<Device> devices);
        void ChangeStatus(KnownStatusDevice device);
    }
}
