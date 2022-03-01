using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace xPowerHub
{
    internal class DAL
    {
        readonly SqliteConnection conn;
        public DAL(string fileName)
        {
            conn = new SqliteConnection("Data Source=" + fileName);
        }
        private string wiztable = "wizdev";
        private string smarttable = "smartdev";
        private void DropWiz()
        {
            conn.Open();
            var comm = conn.CreateCommand();
            comm.CommandText = $"DROP TABLE IF EXISTS {wiztable}";
            comm.ExecuteNonQuery();
            conn.Close();
        }
        private void AddWiz()
        {
            conn.Open();
            var comm = conn.CreateCommand();
            comm.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS " + wiztable + @" (
                    id INTEGER PRIMARY KEY,
                    mac TEXT NOT NULL,
                    ip TEXT NOT NULL,
                    name TEXT NOT NULL
                )
            ";
            comm.ExecuteNonQuery();
            conn.Close();
        }

        public void AddWizDevice(WizDevice dev)
        {
            conn.Open();
            var comm = conn.CreateCommand();
            comm.CommandText =
            @"
                INSERT INTO " + wiztable + @" (mac, ip, name)
                VALUES ($mac, $ip, $name)
            ";
            comm.Parameters.AddWithValue("$mac", dev.MAC);
            comm.Parameters.AddWithValue("$ip", dev.IP);
            comm.Parameters.AddWithValue("$name", dev.Name);
            comm.ExecuteNonQuery();
            conn.Close();
        }
        public List<WizDevice> GetAllWizDevices()
        {
            conn.Open();
            var comm = conn.CreateCommand();
            comm.CommandText =
            @"
                SELECT mac, ip, name FROM " + wiztable + @"
            ";
            List<WizDevice> devices = new();
            using var reader = comm.ExecuteReader();
            while (reader.Read())
            {
                devices.Add(new WizDevice(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
            }
            conn.Close();
            return devices;
        }
        private void DropSmart()
        {
            conn.Open();
            var comm = conn.CreateCommand();
            comm.CommandText = $"DROP TABLE IF EXISTS {smarttable}";
            comm.ExecuteNonQuery();
            conn.Close();
        }
        private void AddSmart()
        {
            conn.Open();
            var comm = conn.CreateCommand();
            comm.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS " + smarttable + @" (
                    id INTEGER PRIMARY KEY,
                    uuid TEXT NOT NULL,
                    name TEXT NOT NULL
                )
            ";
            comm.ExecuteNonQuery();
            conn.Close();
        }
    }
}
