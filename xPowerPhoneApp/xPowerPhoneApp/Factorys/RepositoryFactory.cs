using System;
using System.Collections.Generic;
using System.Text;
using xPowerPhoneApp.Repositorys;
using xPowerPhoneApp.Repositorys.Interfaces;
using xPowerPhoneApp.Repositorys.Mocks;

namespace xPowerPhoneApp.Factorys
{
    internal static class RepositoryFactory
    {
        public static bool UseMock { get; set; }

        private static IPowerRepository _savedPowerRepository;
        public static IPowerRepository CreatePowerRepository(bool forceCreate = false)
        {
            if(forceCreate || _savedPowerRepository == null)
            {
                if (UseMock)
                    _savedPowerRepository = new PowerRepositoryMock();
                else
                    _savedPowerRepository = new PowerRepository(HttpClientFactory.Create());
            }
            return _savedPowerRepository;
        }

        private static IPowerPriceRepository _savedPowerPriceRepository;
        public static IPowerPriceRepository CreatePowerPriceRepository(bool forceCreate = false)
        {
            if (forceCreate || _savedPowerPriceRepository == null)
            {
                _savedPowerPriceRepository = new PowerPriceRepository(HttpClientFactory.Create("http://api.energidataservice.dk/"));
            }
            return _savedPowerPriceRepository;
        }

        private static IDeviceRepository _savedDeviceRepository;
        public static IDeviceRepository CreateDeviceRepository(bool forceCreate = false)
        {
            if (forceCreate || _savedDeviceRepository == null)
            {
                if (UseMock)
                    _savedDeviceRepository = new DeviceRepositoryMock();
                else
                    _savedDeviceRepository = new DeviceRepository(HttpClientFactory.Create());
            }
            return _savedDeviceRepository;
        }

        private static IHubRepository _savedHubRepository;
        public static IHubRepository CreateHubRepository(bool forceCreate = false)
        {
            if (forceCreate || _savedHubRepository == null)
            {
                if (UseMock)
                    _savedHubRepository = new HubRepositoryMock();
                else
                    _savedHubRepository = new HubRepository(HttpClientFactory.Create());
            }
            return _savedHubRepository;
        }

        private static ISmartUnitRepository _savedSmartUnitRepository;
        public static ISmartUnitRepository CreateSmartUnitRepository(bool forceCreate = false)
        {
            if (forceCreate || _savedSmartUnitRepository == null)
            {
                if (UseMock)
                    _savedSmartUnitRepository = new SmartUnitRepositoryMock();
                else
                    _savedSmartUnitRepository = new SmartUnitRepository(HttpClientFactory.Create());
            }
            return _savedSmartUnitRepository;
        }
    }
}
