using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using xPowerPhoneApp.Factorys;
using xPowerPhoneApp.Interfaces;
using xPowerPhoneApp.Models;
using xPowerPhoneApp.Pages;
using xPowerPhoneApp.Repositorys;
using xPowerPhoneApp.Repositorys.Interfaces;

namespace xPowerPhoneApp.ViewModels
{
    internal class DeviceListViewModel : PageViewModel
    {
        private readonly IDeviceRepository _deviceRepository;
        private ObservableCollection<ControlDevice> _device = new ObservableCollection<ControlDevice>();

        public ICommand SwitchStautsCommand { get; set; }
        public ICommand GoToEditDevice { get; set; }

        public ObservableCollection<ControlDevice> Devices
        {
            get => _device;
            set { _device = value; NotifyPropertyChanged(nameof(Devices)); }
        }

        public DeviceListViewModel(IChangePage pageChanger) : base(pageChanger)
        {
            _deviceRepository = RepositoryFactory.CreateDeviceRepository();

            SwitchStautsCommand = new Command(async (dev) => await SwitchStautsAsync(dev as ControlDevice));
            GoToEditDevice = new Command((dev) => {
                _pageChanger.PushPage(new SetDeviceNamePage(dev as ControlDevice));
            });
        }

        public async Task InitializeAsync()
        {
            Devices.Clear();
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
            }
        }

        public async Task SwitchStautsAsync(ControlDevice dev)
        {
            int index = Devices.IndexOf(dev);

            if (!dev.IsStatusKnown) return;

            dev.Status = !dev.Status;
            Devices[index] = dev;
            NotifyPropertyChanged(nameof(Devices));

            bool Changed = await _deviceRepository.UpdateStatus(dev);
            if (!Changed)
            {
                dev.Status = !dev.Status;
            }
            Devices[index] = dev;
            NotifyPropertyChanged(nameof(Devices));
        }
    }
}
