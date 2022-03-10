using Microcharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Factorys;
using xPowerPhoneApp.Repositorys.Interfaces;

namespace xPowerPhoneApp.ViewModels
{
    internal class DailyUsageViewModel : BaseViewModel
    {
        public Chart Chart { get; set; }
        public DateTime Date { get => _day; set
            {
                _ = LoadDay(value);
                _day = value;
                NotifyPropertyChanged(nameof(Date));
            } 
        }

        private DateTime _day;
        private IPowerRepository _powerRepository;

        public DailyUsageViewModel(IPowerRepository powerRepository) : base()
        {
            _powerRepository = powerRepository;
            Date = DateTime.Now;
        }

        private async Task LoadDay(DateTime date)
        {
            Chart = ChartFactory.CreateLineChart((await _powerRepository.GetDaysPowerUsage(date)).ToList(), false);
            NotifyPropertyChanged(nameof(Chart));
        }
    }
}
