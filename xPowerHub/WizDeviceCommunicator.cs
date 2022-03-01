using System.Net;
using System.Net.Sockets;
using System.Text;

namespace xPowerHub;

public class WizDeviceCommunicator
{
    // Wiz devices broadcast on port 38900
    static readonly int broadcastPort = 38900;

    // all wiz devices listen on port 38899
    static readonly int wizPort = 38899;

    static (IPAddress ip, WizMessage? msg) ListenForFirstBeat()
    {
        using var client = new UdpClient(broadcastPort);
        var endPoint = new IPEndPoint(IPAddress.Broadcast, broadcastPort);

        byte[] bytes = client.Receive(ref endPoint);
        IPAddress ip = endPoint.Address;

        string response = Encoding.ASCII.GetString(bytes);
        return (ip, WizMessage.FromJSON(response));
    }

    // Get a device that is broadcasting its first beat
    public static WizDevice GetNewDevice()
    {
        var (ip, msg) = ListenForFirstBeat();

        var mac = msg?.Parameters?.MacAddress ?? string.Empty;
        var dev = new WizDevice(ip.ToString(), mac);
        return dev;
    }

    public static WizMessage SendMessageToDevice(WizDevice device, WizMessage msg)
    {
        using var client = new UdpClient();
        var endPoint = new IPEndPoint(IPAddress.Parse(device.IP), wizPort);

        // Send the command by JSON request
        byte[] buffer = Encoding.ASCII.GetBytes(msg.ToJSON());
        client.Send(buffer, buffer.Length, endPoint);

        // wait for JSON response
        byte[] responseBuffer = client.Receive(ref endPoint);
        string response = Encoding.ASCII.GetString(responseBuffer);
        return WizMessage.FromJSON(response);
    }
}
