using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerHub.Models;

namespace xPowerHub.DataStore
{
    public class PowerDS : IDataStorePower
    {
        private readonly SqliteConnection _conn;
        public PowerDS()
        {
            _conn = new SqliteConnection(@"Data Source=.\xpower.db");
            AddTable();
        }

        public void AddTable()
        {
            _conn.Open();
            var comm = _conn.CreateCommand();
            comm.CommandText =
            @$"
                CREATE TABLE IF NOT EXISTS {nameof(PowerUsage)} (
                    id INTEGER PRIMARY KEY,
                    date NUMERIC NOT NULL,
                    wattHour REAL NOT NULL
                )
            ";
            comm.ExecuteNonQuery();
            _conn.Close();
        }

        public void RemoveTable()
        {
            _conn.Open();
            var comm = _conn.CreateCommand();
            comm.CommandText = $"DROP TABLE IF EXISTS {nameof(PowerUsage)}";
            comm.ExecuteNonQuery();
            _conn.Close();
        }

        public async Task<PowerUsage> GetAsync(DateTime key)
        {
            await _conn.OpenAsync();
            var comm = _conn.CreateCommand();

            comm.CommandText =
            @$"
                SELECT date, wattHour, id FROM {nameof(PowerUsage)} 
                WHERE strftime('%Y-%m-%d %H', [date])=strftime('%Y-%m-%d %H', $date)
            ";

            comm.Parameters.AddWithValue("$date", key);

            using var reader = await comm.ExecuteReaderAsync();
            PowerUsage powers = null;

            while (await reader.ReadAsync())
            {
                powers = new PowerUsage() { Taken = reader.GetDateTime(0), WattHour = reader.GetDouble(1) };
            }

            await _conn.CloseAsync();

            return powers;
        }

        public async Task<IEnumerable<PowerUsage>> GetPowerUsageHourlyAvgAsync(DateTime date)
        {
            await _conn.OpenAsync();
            var comm = _conn.CreateCommand();

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

            await _conn.CloseAsync();

            return powers;
        }

        public async Task<IEnumerable<PowerUsage>> GetPowerUsageWeekdayAvgAsync()
        {
            await _conn.OpenAsync();
            var comm = _conn.CreateCommand();

            comm.CommandText =
            @$"
                SELECT min(date), avg(wattHour) 
                    FROM (
                        SELECT min(date) as date, sum(wattHour) as wattHour 
                        FROM {nameof(PowerUsage)}
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

            await _conn.CloseAsync();

            return powers;
        }

        public async Task<bool> SaveAsync(PowerUsage item)
        {
            await _conn.OpenAsync();
            var comm = _conn.CreateCommand();

            comm.CommandText =
            @$"
                INSERT INTO {nameof(PowerUsage)} (date, wattHour)
                VALUES ($date, $wattHour)
            ";

            comm.Parameters.AddWithValue("$date", item.Taken);
            comm.Parameters.AddWithValue("$wattHour", item.WattHour);

            int inserted = await comm.ExecuteNonQueryAsync();
            await _conn.CloseAsync();

            return inserted == 1;
        }

        public async Task<bool> UpdateAsync(PowerUsage item)
        {
            await _conn.OpenAsync();
            var comm = _conn.CreateCommand();

            comm.CommandText =
            @$"
                UPDATE {nameof(PowerUsage)}
                SET wattHour=$wattHour
                WHERE date=$date
            ";

            comm.Parameters.AddWithValue("$wattHour", item.WattHour);
            comm.Parameters.AddWithValue("$date", item.Taken);

            int updated = await comm.ExecuteNonQueryAsync();
            await _conn.CloseAsync();

            return updated == 1;
        }
    }
}
