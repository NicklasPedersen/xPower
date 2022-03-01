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
        public Device[] GetAll()
        {
            return _deviceManager.GetAll().ToArray();
        }

        /// <summary>
        /// Gets the status of the given devices
        /// </summary>
        /// <param name="devices">The devices where there status will be search for</param>
        /// <returns>A array of KnownStatusDevice</returns>
        [HttpPost("GetStatus")]
        public KnownStatusDevice[] PostStatus([FromBody] Device[] devices)
        {
            return _deviceManager.GetStatus(devices.ToList()).ToArray();
        }

        /// <summary>
        /// Changes the status of the device to the given status
        /// </summary>
        /// <param name="device">The device with the new status</param>
        [HttpPost("ChangeStatus")]
        public void ChangeStatus([FromBody] KnownStatusDevice device)
        {
            _deviceManager.ChangeStatus(device);
        }

        
    }
}
