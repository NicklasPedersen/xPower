using xPowerHub.Models;

namespace xPowerHub.Managers.Interfaces;

public interface IDeviceConnectionManager
{
    void Start();
    void Stop();
    Task<Device[]> GetDevices();
}