using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace xPowerHub.Communicators.SmartThingsMessages
{
    internal class ComponentMain
    {
        [JsonPropertyName("powerMeter")]
        public PowerMeter PowerMeter { get; set; }
        [JsonPropertyName("outlet")]
        public Outlet Outlet { get; set; }
    }
}
