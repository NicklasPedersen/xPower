using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using xPowerPhoneApp.Interfaces;
using xPowerPhoneApp.Repositorys;
using xPowerPhoneApp.Repositorys.Interfaces;
using xPowerPhoneApp.Repositorys.Mocks;
using xPowerPhoneApp.Views;

namespace xPowerPhoneApp.ViewModels
{
    internal class StatViewModel : PageViewModel
    {
        public string PowerLevel { get => _powerLevel.ToString("0.#") + "W"; }
        public string PowerUsage { get => $"Brugt I dag: {_powerUsage.ToString("0.#")}Wh"; }
        public ContentView CurrentView { get; set; }
        public bool IsWeeklyAvgSelectedInv { get => _selected != 0; }
        public bool IsDailyUsageSelectedInv { get => _selected != 1; }
        public bool IsPowerPriceSelectedInv { get => _selected != 2; }
        public ICommand SelectViewCommand { get; set; }

        private int _selected;

        private double _powerLevel;
        private double _powerUsage;
        private bool _getPower = true;
        private Task _getPowerTask;
        private ContentView[] _views;

        private IPowerRepository _powerRepository;
        public StatViewModel(IChangePage pageChanger) : base(pageChanger)
        {
            _powerRepository = new PowerRepository();
            SelectViewCommand = new Command(async (select) => await Task.Run(() => SelectView(int.Parse(select.ToString()))));
            _views = new ContentView[]
            {
                new WeekAvgView(_powerRepository),
                new DailyUsageView(_powerRepository),
                new PriceView(_powerRepository)
            };
            SelectView(0);
        }

        private void SelectView(int selected)
        {
            CurrentView = _views[selected];
            _selected = selected;
            NotifyPropertyChanged(nameof(CurrentView));
            NotifyPropertyChanged(nameof(IsWeeklyAvgSelectedInv));
            NotifyPropertyChanged(nameof(IsDailyUsageSelectedInv));
            NotifyPropertyChanged(nameof(IsPowerPriceSelectedInv));
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
