using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerHub.Models;

namespace xPowerHub.DataStore
{
    public class SqliteDS : IDataStore
    {

        private readonly SqliteConnection conn;
        public SqliteDS(string fileName)
        {
            conn = new SqliteConnection("Data Source=" + fileName);
        }

        public Task<IEnumerable<T>> GetAllAsync<T>(bool forceRefresh = false) where T : ISmart
        {
            switch (typeof(T).Name)
            {
                case nameof(ISmart):
                    List<ISmart> k = new();
                    k = k.Concat(await GetSmartsAsync(forceRefresh)).ToList();
                    k = k.Concat(await GetWizsAsync(forceRefresh)).ToList();
                    return k;
                case nameof(WizDevice):
                    return await GetWizAsync(key);
                case nameof(SmartThingsDevice):
                    break;
                default: throw new NotImplementedException();
            }
        }

        #region Get
        public async Task<T> GetAsync<T>(string key)
        {
            switch (typeof(T).Name)
            {
                case nameof(WizDevice):
                    return await GetWizAsync(key);
                case nameof(SmartThingsDevice):
                    break;
                default: throw new NotImplementedException();
            }
        }

        private async Task<WizDevice> GetWizAsync(string key)
        {

        }


        public async Task<PowerUsage> GetAsync(DateTime date)
        {
            await conn.OpenAsync();
            var comm = conn.CreateCommand();

            comm.CommandText =
            @$"
                SELECT date, wattHour, id FROM {powertable} 
                WHERE strftime('%Y-%m-%d %H', [date])=strftime('%Y-%m-%d %H', $date)
            ";

            comm.Parameters.AddWithValue("$date", date);

            using var reader = await comm.ExecuteReaderAsync();
            PowerUsage powers = null;

            while (await reader.ReadAsync())
            {
                powers = new PowerUsage() { Taken = reader.GetDateTime(0), WattHour = reader.GetDouble(1) };
            }

            await conn.CloseAsync();

            return powers;
        }
        #endregion

        #region Save
        public async Task<bool> SaveAsync(WizDevice item)
        {
            await conn.OpenAsync();
            var comm = conn.CreateCommand();

            comm.CommandText =
            @$"
                INSERT INTO {item.GetType().Name} (mac, ip, name)
                VALUES ($mac, $ip, $name)
            ";

            comm.Parameters.AddWithValue("$mac", item.MAC);
            comm.Parameters.AddWithValue("$ip", item.IP);
            comm.Parameters.AddWithValue("$name", item.Name);

            int inserted = await comm.ExecuteNonQueryAsync();
            await conn.CloseAsync();

            return inserted == 1;
        }

        public async Task<bool> SaveAsync(SmartThingsDevice item)
        {
            await conn.OpenAsync();
            var comm = conn.CreateCommand();

            comm.CommandText =
            @$"
                INSERT INTO {item.GetType().Name} (uuid, name, key)
                VALUES ($uuid, $name, $key)
            ";

            comm.Parameters.AddWithValue("$uuid", item.UUID);
            comm.Parameters.AddWithValue("$name", item.Name);
            comm.Parameters.AddWithValue("$key", item.Key);

            int inserted = await comm.ExecuteNonQueryAsync();
            await conn.CloseAsync();

            return inserted == 1;
        }

        public async Task<bool> SaveAsync(PowerUsage item)
        {
            await conn.OpenAsync();
            var comm = conn.CreateCommand();

            comm.CommandText =
            @$"
                INSERT INTO {item.GetType().Name} (date, wattHour)
                VALUES ($date, $wattHour)
            ";

            comm.Parameters.AddWithValue("$date", item.Taken);
            comm.Parameters.AddWithValue("$wattHour", item.WattHour);

            int inserted = await comm.ExecuteNonQueryAsync();
            await conn.CloseAsync();

            return inserted == 1;
        }
        #endregion

        #region Update
        public async Task<bool> UpdateAsync(WizDevice item)
        {
            await conn.OpenAsync();
            var comm = conn.CreateCommand();

            comm.CommandText =
            @$"
                UPDATE {item.GetType().Name}
                SET name=$name, ip=$ip
                WHERE mac=$mac
            ";

            comm.Parameters.AddWithValue("$name", item.Name);
            comm.Parameters.AddWithValue("$ip", item.IP);
            comm.Parameters.AddWithValue("$mac", item.MAC);

            int updated = await comm.ExecuteNonQueryAsync();
            await conn.CloseAsync();

            return updated == 1;
        }

        public async Task<bool> UpdateAsync(SmartThingsDevice item)
        {
            await conn.OpenAsync();
            var comm = conn.CreateCommand();

            comm.CommandText =
            @$"
                UPDATE {item.GetType().Name}
                SET name=$name
                WHERE uuid=$uuid
            ";

            comm.Parameters.AddWithValue("$name", item.Name);
            comm.Parameters.AddWithValue("$uuid", item.UUID);

            int updated = await comm.ExecuteNonQueryAsync();
            await conn.CloseAsync();

            return updated == 1;
        }

        public Task<bool> UpdateAsync(PowerUsage item)
        {
            await conn.OpenAsync();
            var comm = conn.CreateCommand();

            comm.CommandText =
            @$"
                UPDATE {item.GetType().Name}
                SET wattHour=$wattHour
                WHERE date=$date
            ";

            comm.Parameters.AddWithValue("$wattHour", powerUsage.WattHour);
            comm.Parameters.AddWithValue("$date", powerUsage.Taken);

            int updated = await comm.ExecuteNonQueryAsync();
            await conn.CloseAsync();

            return updated == 1;
        }
        #endregion

        public async Task<IEnumerable<PowerUsage>> GetPowerUsageHourlyAvgAsync(DateTime date)
        {
            await conn.OpenAsync();
            var comm = conn.CreateCommand();

            comm.CommandText =
            @$"
                SELECT min(date), sum(wattHour) FROM {nameof(PowerUsage)} 
                WHERE strftime('%Y-%m-%d', [date])=strftime('%Y-%m-%d', $date)
                GROUP BY strftime('%Y-%m-%d %H', [date]) 
            ";

            comm.Parameters.AddWithValue("$date", date);

            using var reader = await comm.ExecuteReaderAsync();
            List<PowerUsage> powers = new();

            while (await reader.ReadAsync())
            {
                powers.Add(new PowerUsage() { Taken = reader.GetDateTime(0), WattHour = reader.GetDouble(1) });
            }

            await conn.CloseAsync();
            
            return powers;
        }

        public async Task<IEnumerable<PowerUsage>> GetPowerUsageWeekdayAvgAsync()
        {
            await conn.OpenAsync();
            var comm = conn.CreateCommand();

            comm.CommandText =
            @$"
                SELECT min(date), avg(wattHour) 
                    FROM (
                        SELECT min(date) as date, sum(wattHour) as wattHour 
                        FROM {powertable}
                        GROUP BY (strftime('%Y-%m-%d', [date]))
                    )
                    GROUP BY (strftime('%w', [date])) 
                    ORDER BY strftime('%w', [date]) 
            ";

            List<PowerUsage> powers = new();
            using var reader = await comm.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                powers.Add(new PowerUsage() { Taken = reader.GetDateTime(0), WattHour = reader.GetDouble(1) });
            }

            await conn.CloseAsync();

            return powers;
        }
    }
}
