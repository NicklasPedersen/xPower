using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Interfaces;
using xPowerPhoneApp.Models;
using xPowerPhoneApp.Repositorys;

namespace xPowerPhoneApp.ViewModels
{
    internal class ConnectUnitViewModel : BaseViewModel
    {
        private ISmartUnit smartUnitRepo;
        private ObservableCollection<AddDevice> _device = new ObservableCollection<AddDevice>();
        public ObservableCollection<AddDevice> Devices
        {
            get => _device;
            set { _device = value; NotifyPropertyChanged(nameof(Devices)); }
        }
        public ConnectUnitViewModel(IChangePage pageChanger) : base(pageChanger)
        {
            smartUnitRepo = new SmartUnitRepositoryMock();
            Devices.Add(new AddDevice("hello", "world"));
            _ = InitializeAsync();
        }

        public async Task InitializeAsync()
        {

            var devices = await smartUnitRepo.GetDevices();
            foreach (var device in devices)
            {
                Devices.Add(device);
            }
            NotifyPropertyChanged(nameof(Devices));
        }
    }
}
