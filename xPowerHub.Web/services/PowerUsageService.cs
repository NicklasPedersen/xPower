using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Web.services
{
    public class PowerUsageService : IHostedService, IDisposable
    {
        private IPowerManager _powerManager;
        private IDeviceManager _deviceManager;
        private ILogger<PowerUsageService> _logger;
        private Task _TakePowerUsageTask;
        private bool _stop;
        private TimeSpan _interval = TimeSpan.FromMinutes(1);

        public PowerUsageService(IPowerManager powerManager, IDeviceManager deviceManager, ILogger<PowerUsageService> logger)
        {
            _powerManager = powerManager;
            _deviceManager = deviceManager;
            _logger = logger;
        }

        public async Task TakePowerUsageAsync()
        {
            while (!_stop)
            {
                var watts = await _deviceManager.GetAllWattageUsageAsync();
                var powerUsage = watts/(60/_interval.Minutes);

                await _powerManager.AddUsageAsync(new PowerUsage() { WattHour = powerUsage, Taken = DateTime.Now });
                await Task.Delay(_interval);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _stop = false;

            _TakePowerUsageTask = TakePowerUsageAsync();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _stop = true;

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _TakePowerUsageTask.Dispose();
        }
    }
}
