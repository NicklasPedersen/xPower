using Microsoft.Data.Sqlite;

namespace xPowerHub.DataStore;

internal class DAL : IDataStore
{
    readonly SqliteConnection conn;
    public DAL(string fileName)
    {
        conn = new SqliteConnection("Data Source=" + fileName);
    }
    private string wiztable = "wizdev";
    private string smarttable = "smartdev";
    private void DropWizTable()
    {
        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText = $"DROP TABLE IF EXISTS {wiztable}";
        comm.ExecuteNonQuery();
        conn.Close();
    }
    private void AddWizTable()
    {
        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            CREATE TABLE IF NOT EXISTS " + wiztable + @" (
                id INTEGER PRIMARY KEY,
                mac TEXT UNIQUE NOT NULL,
                ip TEXT NOT NULL,
                name TEXT NOT NULL
            )
        ";
        comm.ExecuteNonQuery();
        conn.Close();
    }
    private void DropSmartTable()
    {
        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText = $"DROP TABLE IF EXISTS {smarttable}";
        comm.ExecuteNonQuery();
        conn.Close();
    }
    private void AddSmartTable()
    {
        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
                CREATE TABLE IF NOT EXISTS " + smarttable + @" (
                    id INTEGER PRIMARY KEY,
                    uuid TEXT UNIQUE NOT NULL,
                    name TEXT NOT NULL,
                    key TEXT NOT NULL
                )
        ";
        comm.ExecuteNonQuery();
        conn.Close();
    }
    public void DropTables()
    {
        DropWizTable();
        DropSmartTable();
    }
    public void AddTables()
    {
        AddWizTable();
        AddSmartTable();
    }
    public void DropCreatePopulate()
    {
        DropTables();
        AddTables();
        AddWizAsync(new WizDevice("192.112.31.35", "34:13:33:64", "dev1"));
        AddWizAsync(new WizDevice("192.112.31.34", "34:13:57:64", "dev2"));
        AddWizAsync(new WizDevice("192.112.31.37", "34:13:12:64", "dev3"));
        AddWizAsync(new WizDevice("192.112.31.38", "34:13:54:64", "dev4"));
        AddSmartAsync(new SmartThingsDevice("uuid4-asdad-ij23oi4-asdf", "smdev4", "uuid4-asdad-ij23oi4-asdf"));
    }


    public Task<bool> AddWizAsync(WizDevice item)
    {
        // invalidate our cache
        _wizCache = null;

        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            INSERT INTO " + wiztable + @" (mac, ip, name)
            VALUES ($mac, $ip, $name)
        ";
        comm.Parameters.AddWithValue("$mac", item.MAC);
        comm.Parameters.AddWithValue("$ip", item.IP);
        comm.Parameters.AddWithValue("$name", item.Name);
        int inserted = comm.ExecuteNonQuery();
        conn.Close();
        return Task.FromResult(inserted == 1);
    }

    public Task<bool> UpdateWizAsync(WizDevice item)
    {
        // invalidate our cache
        _wizCache = null;

        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            UPDATE " + smarttable + @"
            SET name=$name, ip=$ip
            WHERE uuid=$uuid
        ";
        comm.Parameters.AddWithValue("$name", item.Name);
        comm.Parameters.AddWithValue("$ip", item.IP);
        comm.Parameters.AddWithValue("$mac", item.MAC);
        int updated = comm.ExecuteNonQuery();
        conn.Close();
        return Task.FromResult(updated == 1);
    }

    public Task<bool> DeleteWizAsync(WizDevice dev)
    {
        // invalidate our cache
        _wizCache = null;

        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            DELETE from " + wiztable + @" WHERE mac=$mac
        ";
        comm.Parameters.AddWithValue("$mac", dev.MAC);
        int deleted = comm.ExecuteNonQuery();
        conn.Close();
        return Task.FromResult(deleted == 1);
    }

    public Task<WizDevice?> GetWizAsync(int id)
    {
        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            SELECT * from " + wiztable + @"
            WHERE id=$id
        ";
        comm.Parameters.AddWithValue("$id", id);
        using var reader = comm.ExecuteReader();
        WizDevice? dev = null;
        if (reader.Read())
        {
            dev = new WizDevice(reader["ip"] as string ?? "", reader["mac"] as string ?? "", reader["name"] as string ?? "");
        }
        conn.Close();
        return Task.FromResult(dev);
    }

    IEnumerable<WizDevice>? _wizCache = null;

    public Task<IEnumerable<WizDevice>> GetWizsAsync(bool forceRefresh = false)
    {
        if (!forceRefresh && _wizCache is not null)
        {
            return Task.FromResult(_wizCache);
        }
        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            SELECT ip, mac, name FROM " + wiztable + @"
        ";
        List<WizDevice> devices = new();
        using var reader = comm.ExecuteReader();
        while (reader.Read())
        {
            devices.Add(new WizDevice(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
        }
        conn.Close();
        _wizCache = devices.AsEnumerable();
        return Task.FromResult(_wizCache);
    }

    public Task<bool> AddSmartAsync(SmartThingsDevice item)
    {
        // invalidate our cache
        _smartCache = null;

        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            INSERT INTO " + smarttable + @" (uuid, name, key)
            VALUES ($uuid, $name, $key)
        ";
        comm.Parameters.AddWithValue("$uuid", item.UUID);
        comm.Parameters.AddWithValue("$name", item.Name);
        comm.Parameters.AddWithValue("$key", item.Key);
        comm.ExecuteNonQuery();
        conn.Close();
        return Task.FromResult(true);
    }

    public Task<bool> UpdateSmartAsync(SmartThingsDevice item)
    {
        // invalidate our cache
        _smartCache = null;

        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            UPDATE " + smarttable + @"
            SET name=$name
            WHERE uuid=$uuid
        ";
        comm.Parameters.AddWithValue("$name", item.Name);
        comm.Parameters.AddWithValue("$uuid", item.UUID);
        int updated = comm.ExecuteNonQuery();
        conn.Close();
        return Task.FromResult(updated == 1);
    }

    public Task<bool> DeleteSmartAsync(SmartThingsDevice dev)
    {
        // invalidate our cache
        _smartCache = null;

        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            DELETE from " + smarttable + @" WHERE uuid=$uuid
        ";
        comm.Parameters.AddWithValue("$uuid", dev.UUID);
        int deleted = comm.ExecuteNonQuery();
        conn.Close();
        return Task.FromResult(deleted == 1);
    }

    public Task<SmartThingsDevice?> GetSmartAsync(int id)
    {
        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            SELECT * from " + smarttable + @"
            WHERE id=$id
        ";
        comm.Parameters.AddWithValue("$id", id);
        using var reader = comm.ExecuteReader();
        SmartThingsDevice? dev = null;
        if (reader.Read())
        {
            dev = new SmartThingsDevice(reader["uuid"] as string, reader["name"] as string, reader["key"] as string);
        }
        conn.Close();
        return Task.FromResult(dev);
    }

    public Task<SmartThingsDevice?> GetSmartAsync(string uuid)
    {
        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            SELECT * from " + smarttable + @"
            WHERE uuid=$uuid
        ";
        comm.Parameters.AddWithValue("$uuid", uuid);
        using var reader = comm.ExecuteReader();
        SmartThingsDevice? dev = null;
        if (reader.Read())
        {
            dev = new SmartThingsDevice(reader["uuid"] as string ?? "", reader["name"] as string ?? "", reader["key"] as string ?? "");
        }
        conn.Close();
        return Task.FromResult(dev);
    }

    IEnumerable<SmartThingsDevice>? _smartCache = null;
    public Task<IEnumerable<SmartThingsDevice>> GetSmartsAsync(bool forceRefresh = false)
    {
        if (!forceRefresh && _smartCache is not null)
        {
            return Task.FromResult(_smartCache);
        }
        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            SELECT uuid, name, key FROM " + smarttable + @"
        ";
        List<SmartThingsDevice> devices = new();
        using var reader = comm.ExecuteReader();
        while (reader.Read())
        {
            devices.Add(new SmartThingsDevice(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
        }
        conn.Close();
        return Task.FromResult(devices.AsEnumerable());
    }
    public Task<IEnumerable<ISmart>> GetAllDevices(bool forceRefresh = false)
    {
        var list = new List<ISmart>();
        list = list.Concat(GetSmartsAsync(forceRefresh).Result).ToList();
        list = list.Concat(GetWizsAsync(forceRefresh).Result).ToList();
        return Task.FromResult(list.AsEnumerable());
    }
}
