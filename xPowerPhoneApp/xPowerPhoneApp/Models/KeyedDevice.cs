using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace xPowerPhoneApp.Models
{
    internal class KeyedDevice : Device
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
    }
}
