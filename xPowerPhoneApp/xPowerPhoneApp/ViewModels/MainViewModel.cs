using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using xPowerPhoneApp.Interfaces;
using xPowerPhoneApp.Pages;
using xPowerPhoneApp.Repositorys;
using xPowerPhoneApp.Repositorys.Mocks;
using xPowerPhoneApp.Repositorys.Interfaces;

namespace xPowerPhoneApp.ViewModels
{
    internal class MainViewModel : PageViewModel
    {
        public string PowerLevel { get => _powerLevel.ToString("0.#") + "W"; }
        public string PowerUsage { get => $"Brugt I dag: {_powerUsage.ToString("0.#")}Wh"; }
        public ICommand GoToAddDevice { get; set; }
        public ICommand GoToListDevice { get; set; }
        public ICommand GoToStatPage { get; set; }

        private double _powerLevel;
        private double _powerUsage;
        private bool _getPower = true;
        private Task _getPowerTask;

        private IPowerRepository _powerRepository;
        public MainViewModel(IChangePage pageChanger) : base(pageChanger)
        {
            _powerRepository = new PowerRepository();

            GoToAddDevice = new Command(() => _pageChanger.PushPage(new AddDevicePage()));
            GoToListDevice = new Command(() => _pageChanger.PushPage(new DeviceListPage()));
            GoToStatPage = new Command(() => _pageChanger.PushPage(new Statpage()));
        }

        public void StartGettingData()
        {
            _getPower = true;
            _getPowerTask = GetPowerAsync();
        }

        public void StopGettingData()
        {
            _getPower = false;
        }

        private async Task GetPowerAsync()
        {
            while (_getPower)
            {
                _powerLevel = await _powerRepository.GetPowerLevel();
                NotifyPropertyChanged(nameof(PowerLevel));

                _powerUsage = await _powerRepository.GetTodaysPowerUsage();
                NotifyPropertyChanged(nameof(PowerUsage));

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}
