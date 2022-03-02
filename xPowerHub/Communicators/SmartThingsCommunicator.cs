using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using xPowerHub.Communicators.SmartThingsMessages;

namespace xPowerHub.Communicators
{
    internal class SmartThingsCommunicator
    {
        private static HttpClient _client;
        private static HttpClient GetHttpClient(string? token = null)
        {
            if (_client == null)
            {
                _client = new HttpClient();
                _client.BaseAddress = new Uri("https://api.smartthings.com/v1/");
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            } 
            if(token != null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return _client;
        }

        public static async Task<SmartThingsDevice[]> GetHubs(string userKey)
        {
            List<SmartThingsDevice> devices = new List<SmartThingsDevice>();
            BaseGet? smartResponse = null;

            HttpResponseMessage response = await GetHttpClient(userKey).GetAsync("devices");

            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                smartResponse = JsonSerializer.Deserialize<BaseGet>(str);
            }

            if(smartResponse != null)
            {
                foreach (var device in smartResponse.Items)
                {
                    if (device.Type == "HUB")
                        devices.Add(new SmartThingsDevice(device.DeviceId, device.Label, userKey));
                }
            }

            return devices.ToArray();
        }

        public static async Task<SmartThingsDevice[]> GetDevices(SmartThingsDevice hub)
        {
            List<SmartThingsDevice> devices = new List<SmartThingsDevice>();
            BaseGet? smartResponse = null;

            HttpResponseMessage response = await GetHttpClient(hub.Key).GetAsync("devices");

            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                smartResponse = JsonSerializer.Deserialize<BaseGet>(str);
            }

            if (smartResponse != null)
            {
                foreach (var device in smartResponse.Items)
                {
                    if (device.ParentDeviceId != null && device.ParentDeviceId == hub.UUID)
                        devices.Add(new SmartThingsDevice(device.DeviceId, device.Label, hub.Key));
                }
            }

            return devices.ToArray();
        }

        public static async Task<bool> GetDeviceOutletStatus(SmartThingsDevice device)
        {
            ComponentHolder? smartResponse = null;

            HttpResponseMessage response = await GetHttpClient(device.Key).GetAsync($"devices/{device.UUID}/status");

            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                smartResponse = JsonSerializer.Deserialize<ComponentHolder>(str);
            }

            if (smartResponse != null)
            {
                return smartResponse.Component.Main.Outlet.Switch.Value == "on";
            }

            return false;
        }

        public static async Task<double> GetDeviceWattStatus(SmartThingsDevice device)
        {
            ComponentHolder? smartResponse = null;

            HttpResponseMessage response = await GetHttpClient(device.Key).GetAsync($"devices/{device.UUID}/status");

            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                smartResponse = JsonSerializer.Deserialize<ComponentHolder>(str);
            }

            if (smartResponse != null)
            {
                return smartResponse.Component.Main.PowerMeter.Power.Value;
            }

            return 0;
        }

        public static async Task<bool> SetStatus(SmartThingsDevice device, bool status)
        {
            var commandholder = new CommandHolder() { 
                Commands = new[]{ 
                    new Command() { 
                        Component = "main", 
                        Capability = "switch",
                        Value = status ? "on" : "off" 
                    } 
                } 
            };

            var content = JsonSerializer.Serialize(commandholder);
            HttpContent httpContent = new StringContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await GetHttpClient(device.Key).PostAsync($"devices/{device.UUID}/commands", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
