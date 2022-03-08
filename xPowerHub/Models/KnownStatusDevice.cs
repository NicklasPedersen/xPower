using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace xPowerHub.Models
{
    public class KnownStatusDevice : Device
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        public KnownStatusDevice() { }

        public KnownStatusDevice(Device device)
        {
            Name = device.Name;
            Id = device.Id;
            Ip = device.Ip;
            ParentId = device.ParentId;
        }
    }
}
