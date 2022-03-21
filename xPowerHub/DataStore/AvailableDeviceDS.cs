using Microsoft.Data.Sqlite;
using xPowerHub.Models;

namespace xPowerHub.DataStore;

public class AvailableDeviceDS : IDataStore<AvailableDevice>
{
    private readonly SqliteConnection _conn;
    public AvailableDeviceDS(string path)
    {
        _conn = new SqliteConnection(@"Data Source=" + path);
        AddTable();
    }
    
    public void AddTable()
    {
        _conn.Open();
        var comm = _conn.CreateCommand();
        comm.CommandText =
            @$"
            CREATE TABLE IF NOT EXISTS {nameof(AvailableDevice)} (
                mac TEXT UNIQUE PRIMARY KEY,
                ip TEXT NOT NULL,
                lastRegistered NUMERIC NOT NULL
            )
        ";
        comm.ExecuteNonQuery();
        _conn.Close();
    }

    public void RemoveTable()
    {
        _conn.Open();
        var comm = _conn.CreateCommand();
        comm.CommandText = $"DROP TABLE IF EXISTS {nameof(AvailableDevice)}";
        comm.ExecuteNonQuery();
        _conn.Close();
    }

    public async Task<bool> SaveAsync(AvailableDevice item)
    {
        await _conn.OpenAsync();
        var comm = _conn.CreateCommand();

        comm.CommandText =
            @$"
                INSERT INTO {nameof(AvailableDevice)} (mac, ip, lastRegistered)
                VALUES ($mac, $ip, $lastRegistered)
            ";

        comm.Parameters.AddWithValue("mac", item.Device.Id);
        comm.Parameters.AddWithValue("$ip", item.Device.Ip);
        comm.Parameters.AddWithValue("$lastRegistered", item.Registered);

        int inserted = await comm.ExecuteNonQueryAsync();
        await _conn.CloseAsync();

        return inserted == 1;
    }

    public async Task<bool> UpdateAsync(AvailableDevice item)
    {
        await _conn.OpenAsync();
        var comm = _conn.CreateCommand();

        comm.CommandText =
            @$"
                UPDATE {nameof(AvailableDevice)}
                SET lastRegistered=$lastRegistered
                WHERE mac=$mac
            ";

        comm.Parameters.AddWithValue("$lastRegistered", item.Registered);
        comm.Parameters.AddWithValue("$mac", item.Device.Id);

        int updated = await comm.ExecuteNonQueryAsync();
        await _conn.CloseAsync();

        return updated == 1;
    }

    public async Task<AvailableDevice> GetAsync(string key)
    {
        await _conn.OpenAsync();
        var comm = _conn.CreateCommand();
        comm.CommandText =
            @$"
                SELECT lastRegistered, mac, ip from {nameof(AvailableDevice)}
                WHERE mac=$mac
            ";
        comm.Parameters.AddWithValue("$mac", key);
        
        await using var reader = await comm.ExecuteReaderAsync();
        
        AvailableDevice? dev = null;
        if (await reader.ReadAsync())
        {
            dev = new AvailableDevice()
            {
                Registered = reader.GetDateTime(0),
                Device = new Device()
                {
                    Id = reader.GetString(1),
                    Ip = reader.GetString(2),
                    Name = reader.GetString(2)
                }
            };
        }
        await _conn.CloseAsync();

        return dev;
    }

    public async Task<IEnumerable<AvailableDevice>> GetAllAsync(bool forceRefresh = false)
    {
        await _conn.OpenAsync();
        var comm = _conn.CreateCommand();
        
        comm.CommandText =
            @$"
                SELECT lastRegistered, mac, ip from {nameof(AvailableDevice)}
            ";
        
        await using var reader = await comm.ExecuteReaderAsync();
        
        var list = new List<AvailableDevice>();
        
        while(await reader.ReadAsync())
        {
            list.Add(new AvailableDevice()
            {
                Registered = reader.GetDateTime(0),
                Device = new Device()
                {
                    Id = reader.GetString(1),
                    Ip = reader.GetString(2),
                    Name = reader.GetString(2)
                }
            });
        }
        
        await _conn.CloseAsync();

        return list;
    }
}