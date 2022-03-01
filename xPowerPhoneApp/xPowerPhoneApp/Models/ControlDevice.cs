using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace xPowerPhoneApp.Models
{
    public class ControlDevice : Device
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }
        [JsonIgnore]
        public bool IsStatusKnown { get; set; }
    }
}
