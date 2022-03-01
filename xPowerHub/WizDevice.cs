using System.Text.Json.Serialization;

namespace xPowerHub;

public class WizDevice : ISmart
{
    [JsonPropertyName("ip")]
    public string IP { get; init; }
    [JsonPropertyName("mac")]
    public string MAC { get; init; }

    public WizDevice(string IP, string MAC)
    {
        this.IP = IP;
        this.MAC = MAC;
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
        return $"(name: WizDevice, mac: {MAC}, ip: {IP})";
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
