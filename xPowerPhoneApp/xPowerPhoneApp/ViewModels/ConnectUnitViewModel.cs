using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using xPowerPhoneApp.Interfaces;
using xPowerPhoneApp.Models;
using xPowerPhoneApp.Repositorys;

namespace xPowerPhoneApp.ViewModels
{
    internal class ConnectUnitViewModel : BaseViewModel
    {
        private ISmartUnitRepository smartUnitRepo;
        private ObservableCollection<AddDevice> _device = new ObservableCollection<AddDevice>();
        private Task _getDevicesTask;
        private bool _run = true;

        public ICommand AddCommand { get; set; }
        public ObservableCollection<AddDevice> Devices
        {
            get => _device;
            set { _device = value; NotifyPropertyChanged(nameof(Devices)); }
        }
        public ConnectUnitViewModel(IChangePage pageChanger) : base(pageChanger)
        {
            AddCommand = new Command(async (mac) => await AddAsync(mac.ToString()));
            smartUnitRepo = new SmartUnitRepository();
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
            _getDevicesTask = GetDevices();
        }

        private async Task GetDevices()
        {
            while (_run)
            {
                var devices = await smartUnitRepo.GetNewDevices(Devices.ToList());
                foreach (var device in devices)
                {
                    Devices.Add(device);
                }
                NotifyPropertyChanged(nameof(Devices));
                await Task.Delay(100);
            }
        }

        public async Task AddAsync(string mac)
        {
            int index = Devices.IndexOf(Devices.FirstOrDefault(d => d.Id == mac));
            var device = Devices[index];
            device.Adding = true;
            Devices[index] = device;
            NotifyPropertyChanged(nameof(Devices));

            bool added = await smartUnitRepo.AddDevice(device);
            device.Adding = false;
            if (added)
            {
                device.Added = true;
            }
            Devices[index] = device;
            NotifyPropertyChanged(nameof(Devices));
        }
    }
}
