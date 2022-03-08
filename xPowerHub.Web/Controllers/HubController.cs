using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Web.Controllers
{
    /// <summary>
    /// Controlls all Hub specific methodes
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HubController : ControllerBase
    {

        private IDeviceManager _deviceManager;

        public HubController(IDeviceManager deviceManager) : base()
        {
            _deviceManager = deviceManager;
        }

        /// <summary>
        /// Ask for all hubs connected to the key
        /// </summary>
        /// <returns>The new Device</returns>
        [HttpPost("GetHubs")]
        public async Task<Device[]> GetNewAsync([FromBody] string key)
        {
            return await _deviceManager.GetAllHubsAsync(key);
        }

        /// <summary>
        /// Addes the Device
        /// </summary>
        [HttpPost("Add")]
        public async Task AddAsync([FromBody] KeyedDevice device)
        {
            await _deviceManager.AddNewDeviceAsync(device);
        }
    }
}
