using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xPowerHub.Managers.Interfaces;
using xPowerHub.Models;

namespace xPowerHub.Web.Controllers
{
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
        public Device[] GetNew([FromBody] string key)
        {
            return _deviceManager.GetAllHubs(key);
        }

        /// <summary>
        /// Addes the Device
        /// </summary>
        [HttpPost("Add")]
        public void Add([FromBody] KeyedDevice device)
        {
            _deviceManager.AddNewDevice(device);
        }
    }
}
