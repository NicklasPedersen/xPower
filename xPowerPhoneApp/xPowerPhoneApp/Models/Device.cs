using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace xPowerPhoneApp.Models
{
    internal class Device
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("ip")]
        public string Ip { get; set; }
        [JsonPropertyName("parentId")]
        public string ParentId { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is Device device)
                return Name == device.Name && Id == device.Id;
            return base.Equals(obj);
        }
    }
}
