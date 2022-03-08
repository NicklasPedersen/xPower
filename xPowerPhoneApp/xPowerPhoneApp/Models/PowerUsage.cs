using System;
using System.Text.Json.Serialization;

namespace xPowerPhoneApp.Models
{
    public class PowerUsage
    {
        [JsonPropertyName("taken")]
        public DateTime Taken { get; set; }

        [JsonPropertyName("wattHour")]
        public double WattHour { get; set; }

        public override string ToString()
        {
            if (WattHour >= 1000000)
                return $"{(WattHour / 1000000).ToString("0.#")}MWh";
            if (WattHour >= 1000)
                return $"{(WattHour/1000).ToString("0.#")}kWh";
            else
                return $"{WattHour.ToString("0.#")}Wh";
        }
    }
}