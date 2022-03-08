using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using xPowerPhoneApp.Interfaces;
using xPowerPhoneApp.Repositorys;
using xPowerPhoneApp.Repositorys.Interfaces;

namespace xPowerPhoneApp.ViewModels
{
    internal class EditDeviceViewModel : PageViewModel
    {
        private string _mac;
        public string Mac
        {
            get => _mac;
            set { _mac = value; NotifyPropertyChanged(nameof(Mac)); }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; NotifyPropertyChanged(nameof(Name)); }
        }
        public string NewName
        {
            get => _device.Name;
            set { _device.Name = value; NotifyPropertyChanged(nameof(Name)); }
        }
        private IDeviceRepository _deviceRepository { get; } = new DeviceRepository();

        private Models.Device _device;
        public ICommand EditNameCommand { get; }

        public EditDeviceViewModel(IChangePage pageChanger, string mac) : base(pageChanger)
        {
            _device = new Models.Device
            {
                Id = mac
            };
            EditNameCommand = new Command(() =>
            {
                _deviceRepository.UpdateName(_device);
            });
            NewName = "";
            Task.Run(async () =>
            {
                var dev = (await _deviceRepository.GetAllDevices()).Where(x => x.Id == mac).First();
                Name = dev.Name;
            });
        }
    }
}
