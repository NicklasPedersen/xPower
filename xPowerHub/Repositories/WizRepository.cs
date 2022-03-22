using System.Net;
using System.Net.Sockets;
using System.Text;
using xPowerHub.Communicators.WizMessages;
using xPowerHub.Repositories.Interfaces;

namespace xPowerHub.Repositories;

public class WizRepository : IWizRepository
{
    public bool Run { get; set; }

    private UdpClient _client;
    
    public WizRepository()
    {
        Run = true;
        _client = new UdpClient(38900);
    }
    
    public void Start(Action<WizDevice> callback)
    {
        _client.BeginReceive(ReceiveCallback, callback);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        var callback = (Action<WizDevice>) ar.AsyncState!;
        IPEndPoint? e = null;
        
        var receiveBytes = _client.EndReceive(ar, ref e);
        var receiveString = Encoding.ASCII.GetString(receiveBytes);
        
        var msg = WizMessage.FromJSON(receiveString);

        var mac = msg?.Parameters?.MacAddress ?? string.Empty;
        var ip = e!.Address.ToString();
        
        callback(new WizDevice(ip, mac, "wiz device"));

        if(Run)
            _client.BeginReceive(ReceiveCallback, callback);
    }
}