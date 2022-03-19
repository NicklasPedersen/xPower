using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;
using xPowerHub.Repositories.Interfaces;

namespace xPowerHub.Managers;

public class DeviceConnectionManager : IDeviceConnectionManager
{
    private readonly IWizRepository _wizRepository;

    public DeviceConnectionManager(IWizRepository wizRepository)
    {
        _wizRepository = wizRepository;
    }
    public void Start()
    {
        _wizRepository.Run = true;
        _wizRepository.Start(test);
    }

    public void Stop()
    {
        _wizRepository.Run = false;
    }

    private void test(WizDevice wizDevice)
    {
    }

    public Task<Device[]> GetDevices()
    {
        throw new NotImplementedException();
    }
}