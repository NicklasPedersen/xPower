﻿using System;
using System.Collections.Generic;
using System.Text;
using xPowerPhoneApp.Repositorys.Shared;

namespace xPowerPhoneApp.Factorys
{
    internal class HttpClientFactory
    {
        private static string _url = "https://192.168.1.142:5001/api/";
        
        private static SharedHttpClient _httpClient;
        public static SharedHttpClient Create(bool forceCreate = false)
        {
            if (forceCreate || _httpClient == null)
                _httpClient = new SharedHttpClient(_url);
            return _httpClient;
        }
        public static SharedHttpClient Create(string baseAddress)
        {
            return new SharedHttpClient(baseAddress);
        }
    }
}
