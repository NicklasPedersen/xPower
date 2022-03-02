using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace xPowerHub.Communicators.SmartThingsMessages
{
    internal class BaseGet
    {
        [JsonPropertyName("items")]
        public BaseDevice[] Items { get; set; }
        [JsonPropertyName("_links")]
        public object Links { get; set; }
    }
}
