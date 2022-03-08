using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace xPowerPhoneApp.Models.EnergiDataService
{
    internal class Record
    {

        [JsonPropertyName("HourDK")]
        public DateTime HourDK { get; set; }
        [JsonPropertyName("SpotPriceDKK")]
        public float SpotPriceDKK { get; set; }
    }
}
