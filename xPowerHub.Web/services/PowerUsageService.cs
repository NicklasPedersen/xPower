using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Web.services
{
    /// <summary>
    /// A Service that Gets current the power usage in W and calulates it to Wh 
    /// </summary>
    public class PowerUsageService : IHostedService, IDisposable
    {
        private IPowerManager _powerManager;
        private IDeviceManager _deviceManager;
        private ILogger<PowerUsageService> _logger;
        private Task _takePowerUsageTask;
        private bool _stop;
        private TimeSpan _interval = TimeSpan.FromMinutes(1);

        public PowerUsageService(IPowerManager powerManager, IDeviceManager deviceManager, ILogger<PowerUsageService> logger)
        {
            _powerManager = powerManager;
            _deviceManager = deviceManager;
            _logger = logger;
        }


        /// <summary>
        /// Gets & saves the power usage
        /// </summary>
        private async Task TakePowerUsageAsync()
        {
            while (!_stop)
            {
                var watts = await _deviceManager.GetAllWattageUsageAsync();
                var powerUsage = watts/(60/_interval.Minutes);

                await _powerManager.AddUsageAsync(new PowerUsage() { WattHour = powerUsage, Taken = DateTime.Now });
                await Task.Delay(_interval);
            }
        }

        /// <summary>
        /// Starts the service that will get & save the power usage
        /// </summary>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _stop = false;

            _takePowerUsageTask = TakePowerUsageAsync();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Stops the service that will get & save the power usage
        /// </summary>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _stop = true;

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _takePowerUsageTask.Dispose();
        }
    }
}
