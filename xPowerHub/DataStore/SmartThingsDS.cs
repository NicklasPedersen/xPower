using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xPowerHub.DataStore
{
    internal class SmartThingsDS : IDataStore<SmartThingsDevice>
    {
        private readonly SqliteConnection _conn;
        private IEnumerable<SmartThingsDevice>? _smartCache = null;
        public SmartThingsDS(string fileName)
        {
            _conn = new SqliteConnection("Data Source=" + fileName);
        }

        public void RemoveTable()
        {
            _conn.Open();
            var comm = _conn.CreateCommand();
            comm.CommandText = $"DROP TABLE IF EXISTS {nameof(SmartThingsDevice)}";
            comm.ExecuteNonQuery();
            _conn.Close();
        }

        public void AddTable()
        {
            _conn.Open();
            var comm = _conn.CreateCommand();
            comm.CommandText =
            @$"
                CREATE TABLE IF NOT EXISTS {nameof(SmartThingsDevice)} (
                    id INTEGER PRIMARY KEY,
                    uuid TEXT UNIQUE NOT NULL,
                    name TEXT NOT NULL,
                    key TEXT NOT NULL
                )
        ";
            comm.ExecuteNonQuery();
            _conn.Close();
        }

        public async Task<IEnumerable<SmartThingsDevice>> GetAllAsync(bool forceRefresh = false)
        {
            if (!forceRefresh && _smartCache is not null)
            {
                return _smartCache;
            }

            await _conn.OpenAsync();
            var comm = _conn.CreateCommand();

            comm.CommandText =
            @"
                SELECT uuid, name, key FROM " + nameof(SmartThingsDevice) + @"
            ";

            List<SmartThingsDevice> devices = new();

            using var reader = await comm.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                devices.Add(new SmartThingsDevice(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
            }

            await _conn.CloseAsync();

            _smartCache = devices;

            return devices;
        }

        public async Task<SmartThingsDevice> GetAsync(string key)
        {
            await _conn.OpenAsync();
            var comm = _conn.CreateCommand();
            comm.CommandText =
            @"
                SELECT * from " + nameof(SmartThingsDevice) + @"
                WHERE uuid=$uuid
            ";
            comm.Parameters.AddWithValue("$uuid", key);
            using var reader = await comm.ExecuteReaderAsync();
            SmartThingsDevice? dev = null;
            if (await reader.ReadAsync())
            {
                dev = new SmartThingsDevice(reader["uuid"] as string ?? "", reader["name"] as string ?? "", reader["key"] as string ?? "");
            }
            await _conn.CloseAsync();

            return dev;
        }


        public async Task<bool> SaveAsync(SmartThingsDevice item)
        {
            await _conn.OpenAsync();
            var comm = _conn.CreateCommand();

            comm.CommandText =
            @$"
                INSERT INTO {item.GetType().Name} (uuid, name, key)
                VALUES ($uuid, $name, $key)
            ";

            comm.Parameters.AddWithValue("$uuid", item.UUID);
            comm.Parameters.AddWithValue("$name", item.Name);
            comm.Parameters.AddWithValue("$key", item.Key);

            int inserted = await comm.ExecuteNonQueryAsync();
            await _conn.CloseAsync();

            return inserted == 1;
        }

        public async Task<bool> UpdateAsync(SmartThingsDevice item)
        {
            await _conn.OpenAsync();
            var comm = _conn.CreateCommand();

            comm.CommandText =
            @$"
                UPDATE {item.GetType().Name}
                SET name=$name
                WHERE uuid=$uuid
            ";

            comm.Parameters.AddWithValue("$name", item.Name);
            comm.Parameters.AddWithValue("$uuid", item.UUID);

            int updated = await comm.ExecuteNonQueryAsync();
            await _conn.CloseAsync();

            return updated == 1;
        }
    }
}
