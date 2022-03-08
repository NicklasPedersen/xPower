using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {

        private IDeviceManager _deviceManager;

        public DeviceController(IDeviceManager deviceManager) : base()
        {
            _deviceManager = deviceManager;
        }

        /// <summary>
        /// Gets all the connected devices
        /// </summary>
        /// <returns>Array of Device</returns>
        [HttpGet("GetAll")]
        public async Task<Device[]> GetAllAsync()
        {
            return (await _deviceManager.GetAllAsync()).ToArray();
        }

        /// <summary>
        /// Gets the status of the given devices
        /// </summary>
        /// <param name="devices">The devices where there status will be search for</param>
        /// <returns>A array of KnownStatusDevice</returns>
        [HttpPost("GetStatus")]
        public async Task<KnownStatusDevice[]> PostStatusAsync([FromBody] Device[] devices)
        {
            return (await _deviceManager.GetStatusAsync(devices.ToList())).ToArray();
        }

        /// <summary>
        /// Changes the status of the device to the given status
        /// </summary>
        /// <param name="device">The device with the new status</param>
        [HttpPost("ChangeStatus")]
        public async Task ChangeStatusAsync([FromBody] KnownStatusDevice device)
        {
            await _deviceManager.ChangeStatusAsync(device);
        }

        /// <summary>
        /// Gets all wattages from all devices that supports it
        /// </summary>
        /// <returns>A number containing the sum of current wattage</returns>
        [HttpGet("GetCurrentWattage")]
        public async Task<double> GetCurrentWattageAsync()
        {
            return await _deviceManager.GetAllWattageUsageAsync();
        }
        /// <summary>
        /// Change the device based on id
        /// </summary>
        /// <returns>Whether or not it was successful</returns>
        [HttpPost("ChangeDevice")] // should this be patch?
        public async Task<bool> ChangeDevice([FromBody] Device d)
        {
            return await _deviceManager.ChangeDevice(d);
        }
    }
}
