using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Models;
using xPowerPhoneApp.Models.EnergiDataService;
using xPowerPhoneApp.Repositorys.Interfaces;
using xPowerPhoneApp.Repositorys.Shared;

namespace xPowerPhoneApp.Repositorys
{
    internal class PowerPriceRepository : IPowerPriceRepository
    {
        public async Task<PowerPrice[]> GetPowerPrice(DateTime date)
        {
            var sqlCall = $"SELECT e.\"HourDK\", e.\"SpotPriceDKK\" " +
                $"FROM \"elspotprices\" as e " +
                $"WHERE e.\"PriceArea\" = 'DK2' AND " +
                $"e.\"HourDK\"  > date '{date.ToString("yyyy-MM-dd")}' AND " +
                $"e.\"HourDK\"  < date  '{date.AddDays(1).ToString("yyyy-MM-dd")}'";

            var outerShell = await SharedHttpClient.PriceInstant.Get<OuterShell>("datastore_search_sql?sql=" + sqlCall);
            var powerPrices = new List<PowerPrice>();

            if (!outerShell.Success) return null;

            foreach(var record in outerShell.Result.Records)
            {
                powerPrices.Add(new PowerPrice() { Hour = record.HourDK, MWhPrice = record.SpotPriceDKK });
            }

            powerPrices = powerPrices.OrderBy(p => p.Hour.Hour).ToList();

            return powerPrices.ToArray();
        }
    }
}
