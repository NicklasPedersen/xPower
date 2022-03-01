namespace xPowerHub;

internal class SmartThingsDevice : ISmart
{
    public string Name { get; init; }
    public string UUID { get; init; }
    public SmartThingsDevice(string uuid, string name)
    {
        UUID = uuid;
        Name = name;
    }

    public bool? GetCurrentState()
    {
        throw new NotImplementedException();
    }

    public bool SetState(bool state)
    {
        throw new NotImplementedException();
    }

    public bool TurnOff()
    {
        throw new NotImplementedException();
    }

    public bool TurnOn()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"(type: SmartThingsDevice, name: {Name}, uuid: {UUID})";
    }
}
