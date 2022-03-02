using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace xPowerHub.Communicators.SmartThingsMessages
{
    internal class BaseDevice
    {
        [JsonPropertyName("deviceId")]
        public string DeviceId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("label")]
        public string Label { get; set; }
        [JsonPropertyName("manufacturerName")]
        public string ManufacturerName { get; set; }
        [JsonPropertyName("presentationId")]
        public string PresentationId { get; set; }
        [JsonPropertyName("parentDeviceId")]
        public string ParentDeviceId { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
