using Microsoft.Data.Sqlite;
using System.Linq;
using xPowerHub.Models;

namespace xPowerHub.DataStore;

public class DAL : IDataStore
{
    readonly SqliteConnection conn;
    public DAL(string fileName)
    {
        conn = new SqliteConnection("Data Source=" + fileName);
        AddTables();
    }
    private string wiztable = "wizdev";
    private string smarttable = "smartdev";
    private string powertable = "powerval";
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
    private void DropPowerTable()
    {
        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText = $"DROP TABLE IF EXISTS {powertable}";
        comm.ExecuteNonQuery();
        conn.Close();
    }
    private void AddPowerTable()
    {
        conn.Open();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            CREATE TABLE IF NOT EXISTS " + powertable + @" (
                id INTEGER PRIMARY KEY,
                date NUMERIC NOT NULL,
                wattHour REAL NOT NULL
            )
        ";
        comm.ExecuteNonQuery();
        conn.Close();
    }
    public void DropTables()
    {
        DropWizTable();
        DropSmartTable();
        DropPowerTable();
    }
    public void AddTables()
    {
        AddWizTable();
        AddSmartTable();
        AddPowerTable();
    }
    public void DropCreatePopulate()
    {
        DropTables();
        AddTables();
        AddWizAsync(new WizDevice("192.112.31.35", "34:13:33:64", "dev1")).Wait();
        AddWizAsync(new WizDevice("192.112.31.34", "34:13:57:64", "dev2")).Wait();
        AddWizAsync(new WizDevice("192.112.31.37", "34:13:12:64", "dev3")).Wait();
        AddWizAsync(new WizDevice("192.112.31.38", "34:13:54:64", "dev4")).Wait();
        AddSmartAsync(new SmartThingsDevice("uuid4-asdad-ij23oi4-asdf", "smdev4", "uuid4-asdad-ij23oi4-asdf")).Wait();
    }

    public async Task<bool> AddWizAsync(WizDevice item)
    {
        // invalidate our cache
        _wizCache = null;

        await conn.OpenAsync();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            INSERT INTO " + wiztable + @" (mac, ip, name)
            VALUES ($mac, $ip, $name)
        ";
        comm.Parameters.AddWithValue("$mac", item.MAC);
        comm.Parameters.AddWithValue("$ip", item.IP);
        comm.Parameters.AddWithValue("$name", item.Name);
        int inserted = await comm.ExecuteNonQueryAsync();
        await conn.CloseAsync();
        return inserted == 1;
    }

    public async Task<bool> UpdateWizAsync(WizDevice item)
    {
        // invalidate our cache
        _wizCache = null;

        await conn.OpenAsync();
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
        int updated = await comm.ExecuteNonQueryAsync();
        await conn.CloseAsync();
        return updated == 1;
    }

    public async Task<bool> DeleteWizAsync(WizDevice dev)
    {
        // invalidate our cache
        _wizCache = null;

        await conn.OpenAsync();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            DELETE from " + wiztable + @" WHERE mac=$mac
        ";
        comm.Parameters.AddWithValue("$mac", dev.MAC);
        int deleted = await comm.ExecuteNonQueryAsync();
        await conn.CloseAsync();
        return deleted == 1;
    }

    public async Task<WizDevice?> GetWizAsync(int id)
    {
        await conn.OpenAsync();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            SELECT * from " + wiztable + @"
            WHERE id=$id
        ";
        comm.Parameters.AddWithValue("$id", id);
        using var reader = await comm.ExecuteReaderAsync();
        WizDevice? dev = null;
        if (await reader.ReadAsync())
        {
            dev = new WizDevice(reader["ip"] as string ?? "", reader["mac"] as string ?? "", reader["name"] as string ?? "");
        }
        await conn.CloseAsync();
        return dev;
    }

    IEnumerable<WizDevice>? _wizCache = null;

    public async Task<IEnumerable<WizDevice>> GetWizsAsync(bool forceRefresh = false)
    {
        if (!forceRefresh && _wizCache is not null)
        {
            return _wizCache;
        }
        await conn.OpenAsync();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            SELECT ip, mac, name FROM " + wiztable + @"
        ";
        List<WizDevice> devices = new();
        using var reader = await comm.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            devices.Add(new WizDevice(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
        }
        await conn.CloseAsync();
        _wizCache = devices.AsEnumerable();
        return _wizCache;
    }

    public async Task<bool> AddSmartAsync(SmartThingsDevice item)
    {
        // invalidate our cache
        _smartCache = null;

        await conn.OpenAsync();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            INSERT INTO " + smarttable + @" (uuid, name, key)
            VALUES ($uuid, $name, $key)
        ";
        comm.Parameters.AddWithValue("$uuid", item.UUID);
        comm.Parameters.AddWithValue("$name", item.Name);
        comm.Parameters.AddWithValue("$key", item.Key);
        int inserted = await comm.ExecuteNonQueryAsync();
        await conn.CloseAsync();
        return inserted == 1;
    }

    public async Task<bool> UpdateSmartAsync(SmartThingsDevice item)
    {
        // invalidate our cache
        _smartCache = null;

        await conn.OpenAsync();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            UPDATE " + smarttable + @"
            SET name=$name
            WHERE uuid=$uuid
        ";
        comm.Parameters.AddWithValue("$name", item.Name);
        comm.Parameters.AddWithValue("$uuid", item.UUID);
        int updated = await comm.ExecuteNonQueryAsync();
        await conn.CloseAsync();
        return updated == 1;
    }

    public async Task<bool> DeleteSmartAsync(SmartThingsDevice dev)
    {
        // invalidate our cache
        _smartCache = null;

        await conn.OpenAsync();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            DELETE from " + smarttable + @" WHERE uuid=$uuid
        ";
        comm.Parameters.AddWithValue("$uuid", dev.UUID);
        int deleted = await comm.ExecuteNonQueryAsync();
        await conn.CloseAsync();
        return deleted == 1;
    }

    public async Task<SmartThingsDevice?> GetSmartAsync(int id)
    {
        await conn.OpenAsync();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            SELECT * from " + smarttable + @"
            WHERE id=$id
        ";
        comm.Parameters.AddWithValue("$id", id);
        using var reader = await comm.ExecuteReaderAsync();
        SmartThingsDevice? dev = null;
        if (await reader.ReadAsync())
        {
            dev = new SmartThingsDevice(reader["uuid"] as string ?? "", reader["name"] as string ?? "", reader["key"] as string ?? "");
        }
        await conn.CloseAsync();
        return dev;
    }
    
    public async Task<SmartThingsDevice?> GetSmartAsync(string uuid)
    {
        await conn.OpenAsync();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            SELECT * from " + smarttable + @"
            WHERE uuid=$uuid
        ";
        comm.Parameters.AddWithValue("$uuid", uuid);
        using var reader = await comm.ExecuteReaderAsync();
        SmartThingsDevice? dev = null;
        if (await reader.ReadAsync())
        {
            dev = new SmartThingsDevice(reader["uuid"] as string ?? "", reader["name"] as string ?? "", reader["key"] as string ?? "");
        }
        await conn.CloseAsync();
        return dev;
    }

    IEnumerable<SmartThingsDevice>? _smartCache = null;
    public async Task<IEnumerable<SmartThingsDevice>> GetSmartsAsync(bool forceRefresh = false)
    {
        if (!forceRefresh && _smartCache is not null)
        {
            return _smartCache;
        }
        await conn.OpenAsync();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            SELECT uuid, name, key FROM " + smarttable + @"
        ";
        List<SmartThingsDevice> devices = new();
        using var reader = await comm.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            devices.Add(new SmartThingsDevice(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
        }
        await conn.CloseAsync();
        return devices;
    }
    
    public async Task<IEnumerable<ISmart>> GetAllDevicesAsync(bool forceRefresh = false)
    {
        List<ISmart> k = new();
        k = k.Concat(await GetSmartsAsync(forceRefresh)).ToList();
        k = k.Concat(await GetWizsAsync(forceRefresh)).ToList();
        return k;
    }

    public async Task<bool> AddPowerStatementAsync(PowerStatement powerStatement)
    {
        // invalidate our cache
        _smartCache = null;

        await conn.OpenAsync();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @"
            INSERT INTO " + powertable + @" (date, wattHour)
            VALUES ($date, $wattHour)
        ";
        comm.Parameters.AddWithValue("$date", powerStatement.Taken);
        comm.Parameters.AddWithValue("$wattHour", powerStatement.WattHour);
        int inserted = await comm.ExecuteNonQueryAsync();
        await conn.CloseAsync();
        return inserted == 1;
    }
   
    public async Task<IEnumerable<PowerStatement>> GetPowerStatementWeekdayAvgAsync()
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
        List<PowerStatement> powers = new();
        using var reader = await comm.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            powers.Add(new PowerStatement() { Taken = reader.GetDateTime(0), WattHour = reader.GetDouble(1) });
        }
        await conn.CloseAsync();
        return powers;
    }
    
    public async Task<IEnumerable<PowerStatement>> GetPowerStatementHourlyAvgAsync(DateTime date)
    {
        await conn.OpenAsync();
        var comm = conn.CreateCommand();
        comm.CommandText =
        @$"
            SELECT min(date), sum(wattHour) FROM {powertable} 
            WHERE strftime('%Y-%m-%d', [date])=strftime('%Y-%m-%d', $datet)
            GROUP BY strftime('%Y-%m-%d %H', [date]) 
        ";
        comm.Parameters.AddWithValue("$datet", date);
        using var reader = await comm.ExecuteReaderAsync();
        List<PowerStatement> powers = new();
        while (await reader.ReadAsync())
        {
            powers.Add(new PowerStatement() { Taken = reader.GetDateTime(0), WattHour = reader.GetDouble(1) });
        }
        await conn.CloseAsync();
        return powers;
    }
}
