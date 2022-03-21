using xPowerHub.DataStore;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;
using xPowerHub.Repositories.Interfaces;

namespace xPowerHub.Managers;

public class DeviceConnectionManager : IDeviceConnectionManager
{
    private readonly IWizRepository _wizRepository;
    private readonly IDataStore<WizDevice> _wisStore;
    private readonly IDataStore<AvailableWiz> _availableWizStore;

    public DeviceConnectionManager(IWizRepository wizRepository, IDataStore<WizDevice> wisStore, IDataStore<AvailableWiz> availableWizStore)
    {
        _wizRepository = wizRepository;
        _wisStore = wisStore;
        _availableWizStore = availableWizStore;
    }
    public void Start()
    {
        _wizRepository.Run = true;
        _wizRepository.Start(RegisterNewDevice);
    }

    public void Stop()
    {
        _wizRepository.Run = false;
    }

    private void RegisterNewDevice(WizDevice wizDevice)
    {
        var knownWiz = _wisStore.GetAsync(wizDevice.MAC).Result;
        
        if(knownWiz != null)
            return;

        var registered = _availableWizStore.GetAsync(wizDevice.MAC).Result;
        
        if (registered != null)
        {
            registered.Registered = DateTime.Now;
            _availableWizStore.UpdateAsync(registered).Wait();
        }
        else
        {
            registered = new AvailableWiz()
            {
                Registered = DateTime.Now,
                Device = new Device()
                {
                    Id = wizDevice.MAC,
                    Ip = wizDevice.IP
                }
            };
            _availableWizStore.SaveAsync(registered).Wait();
        }
    }

    public async Task<Device[]> GetDevices()
    {
        var availableDevices = await _availableWizStore.GetAllAsync();
        var devices = availableDevices.Select(ad => ad.Device);
        return devices.ToArray();
    }
}