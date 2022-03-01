using System.Text.Json;

namespace xPowerHub;

internal class DeviceSerializer
{
    internal class DeviceLists
    {
        public List<WizDevice> WizDevs { get; init; } = new();
    }
    public static string Serialize(ISmart[] smarts)
    {
        DeviceLists d = new();
        foreach (var smart in smarts)
        {
            if (smart is WizDevice dev)
            {
                d.WizDevs.Add(dev);
            }
            else
            {
                string typename = smart.GetType().Name;
                throw new Exception("cannot serialize device of type " + typename);
            }
        }
        return JsonSerializer.Serialize(d);
    }
    public static ISmart[] Deserialize(string json)
    {
        var d = JsonSerializer.Deserialize<DeviceLists>(json)!;
        return d.WizDevs.ToArray();
    }
}
