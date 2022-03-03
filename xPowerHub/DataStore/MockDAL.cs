using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xPowerHub.DataStore;

internal class MockDAL : IDataStore
{
    static SmartThingsDevice[] _stdSmartDevices = {
        new SmartThingsDevice("asdada", "tasd", "asdadas"),
        new SmartThingsDevice("asdada", "tasd", "asdadas"),
        new SmartThingsDevice("asdada", "tasd", "asdadas"),
        new SmartThingsDevice("asdada", "tasd", "asdadas"),
    };
    static WizDevice[] _stdWizDevices = {
        new WizDevice("123.123.123.123", "20:20:20:20", "asdad"),
        new WizDevice("123.123.123.123", "20:20:20:20", "asdad"),
        new WizDevice("123.123.123.123", "20:20:20:20", "asdad"),
    };
    List<SmartThingsDevice> _smartDevices = _stdSmartDevices.ToList();
    List<WizDevice> _wizDevices = _stdWizDevices.ToList();
    public Task<bool> AddSmartAsync(SmartThingsDevice item)
    {
        _smartDevices.Add(item);
        return Task.FromResult(true);
    }

    public Task<bool> AddWizAsync(WizDevice item)
    {
        _wizDevices.Add(item);
        return Task.FromResult(true);
    }

    public Task<bool> DeleteSmartAsync(SmartThingsDevice dev)
    {
        _smartDevices.Add(dev);
        return Task.FromResult(true);
    }

    public Task<bool> DeleteWizAsync(WizDevice dev)
    {
        _wizDevices.Add(dev);
        return Task.FromResult(true);
    }

    public Task<IEnumerable<ISmart>> GetAllDevices(bool forceRefresh = false)
    {
        var list = new List<ISmart>();
        list = list.Concat(GetSmartsAsync(forceRefresh).Result).ToList();
        list = list.Concat(GetWizsAsync(forceRefresh).Result).ToList();
        return Task.FromResult(list.AsEnumerable());
    }

    public Task<SmartThingsDevice?> GetSmartAsync(int id)
    {
        if (id < 0 || id > _smartDevices.Count)
        {
            return Task.FromResult<SmartThingsDevice?>(null);
        }
        return Task.FromResult<SmartThingsDevice?>(_smartDevices[id]);
    }

    public Task<SmartThingsDevice?> GetSmartAsync(string id)
    {
        var dev = _smartDevices.FirstOrDefault(x => x.UUID == id);
        return Task.FromResult(dev);
    }

    public Task<IEnumerable<SmartThingsDevice>> GetSmartsAsync(bool forceRefresh = false)
    {
        return Task.FromResult(_smartDevices.AsEnumerable());
    }

    public Task<WizDevice?> GetWizAsync(int id)
    {
        if (id < 0 || id > _wizDevices.Count)
        {
            return Task.FromResult<WizDevice?>(null);
        }
        return Task.FromResult<WizDevice?>(_wizDevices[id]);
    }

    public Task<IEnumerable<WizDevice>> GetWizsAsync(bool forceRefresh = false)
    {
        return Task.FromResult(_wizDevices.AsEnumerable());
    }

    public Task<bool> UpdateSmartAsync(SmartThingsDevice item)
    {
        var dev = _smartDevices.FindIndex(x => x.UUID == item.UUID);
        if (dev != -1)
        {
            _smartDevices[dev] = item;
        }
        return Task.FromResult(dev != -1);
    }

    public Task<bool> UpdateWizAsync(WizDevice item)
    {
        var dev = _wizDevices.FindIndex(x => x.MAC == item.MAC);
        if (dev != -1)
        {
            _wizDevices[dev] = item;
        }
        return Task.FromResult(dev != -1);
    }
}
