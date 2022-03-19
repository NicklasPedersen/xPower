using xPowerHub.Managers.Interfaces;

namespace xPowerHub.Web.services;

public class DeviceSearchingService: IHostedService, IDisposable
{
    
    private IServiceProvider _services;

    public DeviceSearchingService(IServiceProvider services)
    {
        _services = services;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var deviceConnectionManager =
            scope.ServiceProvider
                .GetRequiredService<IDeviceConnectionManager>();

        deviceConnectionManager.Start();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var deviceConnectionManager =
            scope.ServiceProvider
                .GetRequiredService<IDeviceConnectionManager>();

        deviceConnectionManager.Stop();
    }

    public void Dispose()
    {
    }
}