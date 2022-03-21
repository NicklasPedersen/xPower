using Microsoft.Data.Sqlite;
using xPowerHub.Models;

namespace xPowerHub.DataStore;

public class AvailableWizDS : IDataStore<AvailableWiz>
{
    private readonly SqliteConnection _conn;
    public AvailableWizDS(string path)
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
            CREATE TABLE IF NOT EXISTS {nameof(AvailableWiz)} (
                mac TEXT UNIQUE PRIMARY KEY,
                ip TEXT NOT NULL,
                date NUMERIC NOT NULL
            )
        ";
        comm.ExecuteNonQuery();
        _conn.Close();
    }

    public void RemoveTable()
    {
        _conn.Open();
        var comm = _conn.CreateCommand();
        comm.CommandText = $"DROP TABLE IF EXISTS {nameof(AvailableWiz)}";
        comm.ExecuteNonQuery();
        _conn.Close();
    }

    public async Task<bool> SaveAsync(AvailableWiz item)
    {
        await _conn.OpenAsync();
        var comm = _conn.CreateCommand();

        comm.CommandText =
            @$"
                INSERT INTO {item.GetType().Name} (mac, ip, date)
                VALUES ($mac, $ip, $date)
            ";

        comm.Parameters.AddWithValue("mac", item.Device.Id);
        comm.Parameters.AddWithValue("$ip", item.Device.Ip);
        comm.Parameters.AddWithValue("$date", item.Registered);

        int inserted = await comm.ExecuteNonQueryAsync();
        await _conn.CloseAsync();

        return inserted == 1;
    }

    public async Task<bool> UpdateAsync(AvailableWiz item)
    {
        await _conn.OpenAsync();
        var comm = _conn.CreateCommand();

        comm.CommandText =
            @$"
                UPDATE {item.GetType().Name}
                SET date=$date
                WHERE mac=$mac
            ";

        comm.Parameters.AddWithValue("$date", item.Registered);
        comm.Parameters.AddWithValue("$mac", item.Device.Id);

        int updated = await comm.ExecuteNonQueryAsync();
        await _conn.CloseAsync();

        return updated == 1;
    }

    public async Task<AvailableWiz> GetAsync(string key)
    {
        await _conn.OpenAsync();
        var comm = _conn.CreateCommand();
        comm.CommandText =
            @$"
                SELECT date, mac, ip from {nameof(AvailableWiz)}
                WHERE mac=$mac
            ";
        comm.Parameters.AddWithValue("$mac", key);
        
        await using var reader = await comm.ExecuteReaderAsync();
        
        AvailableWiz? dev = null;
        if (await reader.ReadAsync())
        {
            dev = new AvailableWiz()
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

    public async Task<IEnumerable<AvailableWiz>> GetAllAsync(bool forceRefresh = false)
    {
        await _conn.OpenAsync();
        var comm = _conn.CreateCommand();
        
        comm.CommandText =
            @$"
                SELECT date, mac, ip from {nameof(AvailableWiz)}
            ";
        
        await using var reader = await comm.ExecuteReaderAsync();
        
        var list = new List<AvailableWiz>();
        
        while(await reader.ReadAsync())
        {
            list.Add(new AvailableWiz()
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