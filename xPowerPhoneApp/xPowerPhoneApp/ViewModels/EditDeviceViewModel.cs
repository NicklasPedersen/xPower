using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using xPowerPhoneApp.Factorys;
using xPowerPhoneApp.Interfaces;
using xPowerPhoneApp.Models;
using xPowerPhoneApp.Repositorys;
using xPowerPhoneApp.Repositorys.Interfaces;

namespace xPowerPhoneApp.ViewModels
{
    internal class EditDeviceViewModel : PageViewModel
    {
        public string Id
        {
            get => _device.Id;
            set { _device.Id = value; NotifyPropertyChanged(); }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; NotifyPropertyChanged(); }
        }
        public string NewName
        {
            get => _device.Name;
            set { _device.Name = value; NotifyPropertyChanged(); }
        }
        private IDeviceRepository _deviceRepository;

        private Models.Device _device;
        public ICommand EditNameCommand { get; }

        public EditDeviceViewModel(IChangePage pageChanger, ControlDevice device) : base(pageChanger)
        {
            _deviceRepository = RepositoryFactory.CreateDeviceRepository();

            Name = device.Name;
            _device = device;
            EditNameCommand = new Command(() =>
            {
                _deviceRepository.UpdateName(_device);
                pageChanger.PopPage();
            });
        }
    }
}
