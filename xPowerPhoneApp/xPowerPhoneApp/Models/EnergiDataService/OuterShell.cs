using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace xPowerPhoneApp.Models.EnergiDataService
{
    internal class OuterShell
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("result")]
        public RecordsHolder Result { get; set; }
    }
}
