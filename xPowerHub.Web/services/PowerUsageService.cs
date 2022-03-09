using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Web.services
{
    /// <summary>
    /// A Service that Gets current the power usage in W and calulates it to Wh 
    /// </summary>
    public class PowerUsageService : IHostedService, IDisposable
    {
        private IServiceProvider _services;
        private ILogger<PowerUsageService> _logger;
        private Task? _takePowerUsageTask;
        private bool _stop;
        private TimeSpan _interval = TimeSpan.FromMinutes(1);

        public PowerUsageService(IServiceProvider services, ILogger<PowerUsageService> logger)
        {
            _services = services;
            _logger = logger;
        }


        /// <summary>
        /// Gets & saves the power usage
        /// </summary>
        private async Task TakePowerUsageAsync()
        {
            double lastWatt = -1;
            while (!_stop)
            {
                using (var scope = _services.CreateScope())
                {
                    var deviceManager =
                        scope.ServiceProvider
                            .GetRequiredService<IDeviceManager>();
                    var powerManager =
                        scope.ServiceProvider
                            .GetRequiredService<IPowerManager>();

                    var watts = await deviceManager.GetAllWattageUsageAsync();

                    if (lastWatt < 0)
                        lastWatt = watts;

                    var powerUsage = (watts + lastWatt) / 2 / (60 / _interval.Minutes);

                    lastWatt = watts;

                    await powerManager.AddUsageAsync(new PowerUsage() { WattHour = powerUsage, Taken = DateTime.Now });

                }
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
            if(_takePowerUsageTask != null)
                _takePowerUsageTask.Dispose();
        }
    }
}
