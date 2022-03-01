﻿using System;
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
    internal class DeviceListViewModel : BaseViewModel
    {
        private readonly IDeviceRepository _deviceRepository;
        private ObservableCollection<ControlDevice> _device = new ObservableCollection<ControlDevice>();

        public ICommand SwitchStautsCommand { get; set; }
        public ObservableCollection<ControlDevice> Devices
        {
            get => _device;
            set { _device = value; NotifyPropertyChanged(nameof(Devices)); }
        }

        public DeviceListViewModel(IChangePage pageChanger) : base(pageChanger)
        {
            SwitchStautsCommand = new Command(async (mac) => await SwitchStautsAsync(mac.ToString()));
            _deviceRepository = new DeviceRepositoryMock();
            _ = InitializeAsync();
        }

        public async Task InitializeAsync()
        {

            var devices = await _deviceRepository.GetAllDevices();
            foreach (var device in devices)
            {
                Devices.Add(device);
            }
            NotifyPropertyChanged(nameof(Devices));

            devices = await _deviceRepository.GetStatusOnDevices(Devices.ToList());
            for (int j = 0; j < devices.Count; j++)
            {
                Devices[j] = devices[j];
                await Task.Delay(100);
            }
        }

        public async Task SwitchStautsAsync(string mac)
        {
            int index = Devices.IndexOf(Devices.FirstOrDefault(d => d.Mac == mac));
            var device = Devices[index];

            if (!device.IsStatusKnown) return;

            device.Status = !device.Status;
            Devices[index] = device;
            NotifyPropertyChanged(nameof(Devices));

            bool Changed = await _deviceRepository.UpdateStatus(device);
            if (!Changed)
            {
                device.Status = !device.Status;
            }
            Devices[index] = device;
            NotifyPropertyChanged(nameof(Devices));
        }
    }
}