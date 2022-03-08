namespace xPowerHub.DataStore;

public class WizDS : IDataStore<WizDevice>
{
    public async void AddTable()
    {
        conn.Open();
        var comm = conn.CreateCommand();
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
        conn.Close();
    }

    public async void RemoveTable()
    {
        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText = $"DROP TABLE IF EXISTS {nameof(WizDevice)}";
        comm.ExecuteNonQuery();
        conn.Close();
    }

    public async Task<IEnumerable<WizDevice>> GetAllAsync(bool forceRefresh = false)
    {
        await conn.OpenAsync();
        var comm = conn.CreateCommand();
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
        await conn.OpenAsync();
        var comm = conn.CreateCommand();

        comm.CommandText =
        @"
            SELECT ip, mac, name from " + wiztable + @"
            WHERE id=$id
        ";
        comm.Parameters.AddWithValue("$id", id);
        using var reader = await comm.ExecuteReaderAsync();

        WizDevice? dev = null;
        if (await reader.ReadAsync())
        {
            dev = new WizDevice(reader.GetString(0), reader.GetString(1), reader.GetString(2));
        }

        await conn.CloseAsync();
        return dev;
    }

    public Task<bool> SaveAsync(WizDevice item)
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

    public Task<bool> UpdateAsync(WizDevice item)
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
}