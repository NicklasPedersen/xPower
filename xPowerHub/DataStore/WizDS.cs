using Microsoft.Data.Sqlite;

namespace xPowerHub.DataStore;

public class WizDS : IDataStore<WizDevice>
{
    private readonly SqliteConnection _conn;
    private IEnumerable<SmartThingsDevice>? _smartCache = null;

    public WizDS(string path)
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
            CREATE TABLE IF NOT EXISTS {nameof(WizDevice)} (
                id INTEGER PRIMARY KEY,
                mac TEXT UNIQUE NOT NULL,
                ip TEXT NOT NULL,
                name TEXT NOT NULL
            )
        ";
        comm.ExecuteNonQuery();
        _conn.Close();
    }

    public void RemoveTable()
    {
        _conn.Open();
        var comm = _conn.CreateCommand();
        comm.CommandText = $"DROP TABLE IF EXISTS {nameof(WizDevice)}";
        comm.ExecuteNonQuery();
        _conn.Close();
    }

    public async Task<IEnumerable<WizDevice>> GetAllAsync(bool forceRefresh = false)
    {
        await _conn.OpenAsync();
        var comm = _conn.CreateCommand();
        comm.CommandText =
        @$"
            SELECT ip, mac, name FROM {nameof(WizDevice)}
        ";
        List<WizDevice> devices = new();
        using var reader = await comm.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            devices.Add(new WizDevice(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
        }
        return devices;
    }

    public async Task<WizDevice> GetAsync(string key)
    {
        await _conn.OpenAsync();
        var comm = _conn.CreateCommand();

        comm.CommandText =
        @"
            SELECT ip, mac, name from " + nameof(WizDevice) + @"
            WHERE mac=$mac
        ";
        comm.Parameters.AddWithValue("$mac", key);
        using var reader = await comm.ExecuteReaderAsync();

        WizDevice? dev = null;
        if (await reader.ReadAsync())
        {
            dev = new WizDevice(reader.GetString(0), reader.GetString(1), reader.GetString(2));
        }

        await _conn.CloseAsync();

        return dev;
    }

    public async Task<bool> SaveAsync(WizDevice item)
    {
        await _conn.OpenAsync();
        var comm = _conn.CreateCommand();

        comm.CommandText =
        @$"
            INSERT INTO {item.GetType().Name} (mac, ip, name)
            VALUES ($mac, $ip, $name)
        ";

        comm.Parameters.AddWithValue("$mac", item.MAC);
        comm.Parameters.AddWithValue("$ip", item.IP);
        comm.Parameters.AddWithValue("$name", item.Name);

        int inserted = await comm.ExecuteNonQueryAsync();
        await _conn.CloseAsync();

        return inserted == 1;
    }

    public async Task<bool> UpdateAsync(WizDevice item)
    {
        await _conn.OpenAsync();
        var comm = _conn.CreateCommand();

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
        await _conn.CloseAsync();

        return updated == 1;
    }
}