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
using xPowerPhoneApp.Repositorys;
using xPowerPhoneApp.Repositorys.Interfaces;

namespace xPowerPhoneApp.ViewModels
{
    internal class ConnectHubViewModel : PageViewModel
    {
        private readonly IHubRepository _hubRepo;
        private ObservableCollection<AddDevice> _device = new ObservableCollection<AddDevice>();
        private string _currentKey;

        public ICommand AddCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ObservableCollection<AddDevice> Devices
        {
            get => _device;
            set { _device = value; NotifyPropertyChanged(nameof(Devices)); }
        }
        public string Key { get; set; }

        public ConnectHubViewModel(IChangePage pageChanger) : base(pageChanger)
        {
            _hubRepo = RepositoryFactory.CreateHubRepository();

            AddCommand = new Command(async (id) => await AddAsync(id.ToString()));
            SearchCommand = new Command(async () => await SearchAsync());
        }

        private async Task AddAsync(string id)
        {
            int index = Devices.IndexOf(Devices.FirstOrDefault(d => d.Id == id));
            var device = Devices[index];
            device.Adding = true;
            Devices[index] = device;
            NotifyPropertyChanged(nameof(Devices));

            var keyDevice = new KeyedDevice()
            {
                Ip = device.Ip,
                Id = device.Id,
                ParentId = device.ParentId,
                Key = _currentKey,
                Name = device.Name
            };

            bool added = await _hubRepo.AddHub(keyDevice);
            device.Adding = false;
            if (added)
            {
                device.Added = true;
            }
            Devices[index] = device;
            NotifyPropertyChanged(nameof(Devices));
        }
        
        private async Task SearchAsync()
        {
            var devices = await _hubRepo.GetHubs(Key);
            _currentKey = Key;
            Devices.Clear();
            foreach (var device in devices)
            {
                Devices.Add(new AddDevice(device));
            }
            NotifyPropertyChanged(nameof(Devices));
        }
    }
}
