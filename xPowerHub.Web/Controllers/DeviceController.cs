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

        [HttpGet("GetAll")]
        public Device[] GetAll()
        {
            return _deviceManager.GetAll().ToArray();
        }

        [HttpPost("GetStatus")]
        public KnownStatusDevice[] PostStatus([FromBody] Device[] devices)
        {
            return _deviceManager.GetStatus(devices.ToList()).ToArray();
        }

        [HttpPost("ChangeStatus")]
        public void ChangeStatus([FromBody] KnownStatusDevice device)
        {
            _deviceManager.ChangeStatus(device);
        }

        
    }
}
