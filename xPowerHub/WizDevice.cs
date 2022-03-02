using System.Text.Json.Serialization;
using xPowerHub.Communicators;

namespace xPowerHub;

public class WizDevice : ISmart
{
    [JsonPropertyName("ip")]
    public string IP { get; init; }
    [JsonPropertyName("mac")]
    public string MAC { get; init; }
    [JsonPropertyName("name")]
    public string Name { get; init; }

    public WizDevice(string IP, string MAC, string name = "")
    {
        this.IP = IP;
        this.MAC = MAC;
        this.Name = name;
    }

    public bool? GetCurrentState()
    {
        
        var state = SendMessage(WizMessage.GetPilot()).Result?.State;
        return state;
    }

    public WizMessage SendMessage(WizMessage msg)
    {
        return WizDeviceCommunicator.SendMessageToDevice(this, msg);
    }

    public bool SetState(bool state)
    {
        return SendMessage(WizMessage.SetState(state)).Error == null;
    }

    public override string ToString()
    {
        return $"(type: WizDevice, name: {Name}, mac: {MAC}, ip: {IP})";
    }

    public bool TurnOff()
    {
        return SetState(false);
    }

    public bool TurnOn()
    {
        return SetState(true);
    }
}
