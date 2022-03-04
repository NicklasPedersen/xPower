using xPowerHub.Communicators;

namespace xPowerHub;

public class SmartThingsDevice : ISmart
{
    public string Name { get; init; }
    public string UUID { get; init; }
    public string Key { get; init; }
    public SmartThingsDevice(string uuid, string name, string key)
    {
        UUID = uuid;
        Name = name;
        Key = key;
    }

    public bool? GetCurrentState()
    {
        var getStatusTask = SmartThingsCommunicator.GetDeviceOutletStatus(this);
        getStatusTask.Wait();
        return getStatusTask.Result;
    }

    public bool SetState(bool state)
    {
        var setStatusTask = SmartThingsCommunicator.SetStatus(this, state);
        setStatusTask.Wait();
        return setStatusTask.Result;
    }

    public double GetWatt()
    {
        return SmartThingsCommunicator.GetDeviceWattStatus(this).Result;
    }

    public bool TurnOff()
    {
        return SetState(false);
    }

    public bool TurnOn()
    {
        return SetState(true);
    }

    public override string ToString()
    {
        return $"(type: SmartThingsDevice, name: {Name}, uuid: {UUID})";
    }
}
