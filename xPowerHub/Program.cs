using System.Net;
using System.Net.Sockets;
using System.Text;
using ConsoleApp4;
using System.Text.Json;
using System.Text.Json.Serialization;

//static void K()
//{
//    using (var client = new UdpClient())
//    {
//        var endPoint = new IPEndPoint(IPAddress.Parse("192.168.1.50"), 38899);
//        var opts = new JsonSerializerOptions();
//        opts.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
//        string serializedRequest = JsonSerializer.Serialize(new WizMessage { Method = EMethod.getModelConfig.ToString() }, opts);
//        byte[] buffer = Encoding.ASCII.GetBytes(serializedRequest);
//        client.Send(buffer, buffer.Length, endPoint);
//        byte[] responseBuffer = client.Receive(ref endPoint);
//        string response = System.Text.Encoding.ASCII.GetString(responseBuffer, 0, responseBuffer.Length);
//        Console.WriteLine(response);
//        var msg = JsonSerializer.Deserialize<WizMessage?>(response);
//        //serializedRequest = JsonSerializer.Serialize(WizMessage.SetState((!msg?.Result?.State) ?? false), opts);
//        //buffer = Encoding.ASCII.GetBytes(serializedRequest);
//        //client.Send(buffer, buffer.Length, endPoint);
//        //responseBuffer = client.Receive(ref endPoint);
//        //response = System.Text.Encoding.ASCII.GetString(responseBuffer, 0, responseBuffer.Length);
//        //Console.WriteLine(response);
//        //    Thread.Sleep(1000);
//    }
//}

static (IPAddress ip, byte[] bytes) ListenForBroadcast(int port)
{
    using var client = new UdpClient(port);
    var endPoint = new IPEndPoint(IPAddress.Broadcast, port);    
    
    byte[] bytes = client.Receive(ref endPoint);
    return (endPoint.Address, bytes);
}

static (IPAddress ip, WizMessage? msg) ListenForFirstBeat()
{
    // Wiz devices broadcast on port 38900
    const int broadcastPort = 38900;

    var responseBuf = ListenForBroadcast(broadcastPort);
    string response = Encoding.ASCII.GetString(responseBuf.bytes);
    return (responseBuf.ip, WizMessage.FromJSON(response));
}

static WizMessage? SendMessageToWiz(WizMessage message, IPAddress address)
{
    // all wiz devices listen on port 38899
    const int wizPort = 38899;

    using var client = new UdpClient();
    var endPoint = new IPEndPoint(address, wizPort);

    // Send the command by JSON request
    byte[] buffer = Encoding.ASCII.GetBytes(message.ToJSON());
    client.Send(buffer, buffer.Length, endPoint);
    // wait for JSON response
    byte[] responseBuffer = client.Receive(ref endPoint);
    string response = Encoding.ASCII.GetString(responseBuffer);
    return WizMessage.FromJSON(response);
}

// Write device to file
void WriteDevice(string file, WizDevice device)
{
    var opts = new JsonSerializerOptions();
    opts.WriteIndented = true;

    File.WriteAllText(file, JsonSerializer.Serialize(device, opts));
}

// Get a device that is broadcasting its first beat
WizDevice GetDevice()
{
    var first = ListenForFirstBeat();

    var dev = new WizDevice
    {
        ip = first.ip.ToString(),
        mac = first.msg?.Params?.MacAddress ?? string.Empty,
    };
    return dev;
}

WizDevice GetDeviceFromFile(string file)
{
    return JsonSerializer.Deserialize<WizDevice>(File.ReadAllText(file))!;
}

var file = @"..\..\..\data.json";

WriteDevice(file, GetDevice());

var dev = GetDeviceFromFile(file);
IPAddress devip = IPAddress.Parse(dev.ip);
var state = SendMessageToWiz(WizMessage.GetPilot(), devip);

var msg = SendMessageToWiz(WizMessage.SetState(!state?.Result?.State ?? false), devip);
//Thread.Sleep(1000);
