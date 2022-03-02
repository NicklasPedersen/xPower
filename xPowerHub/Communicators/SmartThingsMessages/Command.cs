using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace xPowerHub.Communicators.SmartThingsMessages
{
    internal class Command
    {
        [JsonPropertyName("component")]
        public string Component { get; set; }
        [JsonPropertyName("capability")]
        public string Capability { get; set; }
        [JsonPropertyName("command")]
        public string Value { get; set; }
    }
}