using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace xPowerPhoneApp.Models
{
    public class Device
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is Device device)
                return Name == device.Name && Id == device.Id;
            return base.Equals(obj);
        }
    }
}
