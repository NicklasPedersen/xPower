using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace xPowerHub.Communicators.SmartThingsMessages
{
    internal class Component
    {
        [JsonPropertyName("main")]
        public ComponentMain Main { get; set; }
    }
}
