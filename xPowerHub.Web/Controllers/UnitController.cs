using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {

        private IDeviceManager _deviceManager;

        public UnitController(IDeviceManager deviceManager) : base()
        {
            _deviceManager = deviceManager;
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
    }
}
