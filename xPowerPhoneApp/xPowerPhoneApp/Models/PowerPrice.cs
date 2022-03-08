using System;
using System.Collections.Generic;
using System.Text;

namespace xPowerPhoneApp.Models
{
    internal class PowerPrice
    {
        public DateTime Hour { get; set; }
        public float MWhPrice { get; set; }

        public override string ToString()
        {
            var kWhPriceDkk = MWhPrice / 1000;
            return $"{kWhPriceDkk.ToString("0.#")}kr./kWh";
        }
    }
}
