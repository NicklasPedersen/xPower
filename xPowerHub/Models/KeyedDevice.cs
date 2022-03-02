using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace xPowerHub.Models
{
    public class KeyedDevice : Device
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
    }
}
