using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace xPowerPhoneApp.Repositorys.Shared
{
    internal class SharedHttpClient
    {
        private HttpClient _client;

        private static SharedHttpClient _instants;

        public static SharedHttpClient Instants { 
            get 
            { 
                if (_instants == null) 
                    _instants = new SharedHttpClient(); 
                return _instants; 
            } 
        }

        public SharedHttpClient()
        {
            SetupClient();
        }

        /// <summary>
        /// Sets up the http client with the base data
        /// </summary>
        private void SetupClient()
        {
            if (_client != null) return;
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            _client = new HttpClient(clientHandler);
            _client.BaseAddress = new Uri("http://192.168.1.122:5075/api/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Sends a post serializes the response
        /// </summary>
        /// <typeparam name="T">The response class</typeparam>
        /// <param name="path">The api path</param>
        /// <param name="obj">The object that will be put in the content</param>
        /// <returns>Serialized Response in the given type</returns>
        /// <exception cref="Exception">Throws if the post failes</exception>
        public async Task<T> Post<T>(string path, object obj)
        {
            var content = JsonSerializer.Serialize(obj);
            HttpContent httpContent = new StringContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _client.PostAsync(path, httpContent);
            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(str);
            }
            throw new Exception($"Failed to Post to server code: {response.StatusCode}");
        }

        /// <summary>
        /// Sends a post
        /// </summary>
        /// <param name="path">The api path</param>
        /// <param name="obj">The object that will be put in the content</param>
        /// <returns>If the post worked or not</returns>
        public async Task<bool> Post(string path, object obj)
        {
            var content = JsonSerializer.Serialize(obj);
            HttpContent httpContent = new StringContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _client.PostAsync(path, httpContent);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sends a get serialzes the responese
        /// </summary>
        /// <typeparam name="T">The response class</typeparam>
        /// <param name="path">The api path</param>
        /// <returns>Serialized Response in the given type</returns>
        /// <exception cref="Exception">Throws if the get failes</exception>
        public async Task<T> Get<T>(string path)
        {
            HttpResponseMessage response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(str);
            }
            throw new Exception($"Failed to Post to server code: {response.StatusCode}");
        }
    }
}
