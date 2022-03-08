using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace xPowerPhoneApp.Models.EnergiDataService
{
    internal class RecordsHolder
    {

        [JsonPropertyName("records")]
        public Record[] Records { get; set; }
        [JsonPropertyName("sql")]
        public string Sql { get; set; }
    }
}
