using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Web.Controllers
{
    /// <summary>
    /// Controlls methodes for devices that don't use a hub
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {

        private IDeviceManager _deviceManager;
        private IDeviceConnectionManager _connectionManager;

        public UnitController(IDeviceManager deviceManager, IDeviceConnectionManager connectionManager) : base()
        {
            _deviceManager = deviceManager;
            _connectionManager = connectionManager;
        }

        /// <summary>
        /// Goes and starts listening for first beat
        /// </summary>
        /// <returns>The new Device</returns>
        [HttpGet("GetNew")]
        public Device? GetNew()
        {
            return _deviceManager.GetNewDevice();
        }

        /// <summary>
        /// Addes the Device
        /// </summary>
        [HttpPost("Add")]
        public async Task AddAsync([FromBody] Device device)
        {
            await _deviceManager.AddNewDeviceAsync(device);
        }

        [HttpGet("GetNewDevices")]
        public async Task<Device[]> GetNewDevices()
        {
            return await _connectionManager.GetDevicesAsync();
        }
    }
}
