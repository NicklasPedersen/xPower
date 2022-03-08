using Microcharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Factorys;
using xPowerPhoneApp.Repositorys;
using xPowerPhoneApp.Repositorys.Interfaces;

namespace xPowerPhoneApp.ViewModels
{
    internal class PriceViewModel : BaseViewModel
    {
        public Chart Chart { get; set; }
        public Chart DailyChart { get; set; }
        public DateTime Date
        {
            get => _day; set
            {
                _ = LoadDay(value);
                _day = value;
                NotifyPropertyChanged(nameof(Date));
            }
        }

        private DateTime _day;

        private IPowerRepository _powerRepository;
        private IPowerPriceRepository _powerPriceRepository;
        public PriceViewModel(IPowerRepository powerRepository) : base()
        {
            _powerRepository = powerRepository;
            _powerPriceRepository = new PowerPriceRepository();
            Date = DateTime.Now;
        }

        private async Task LoadDay(DateTime date)
        {
            var priceChartLoad = Task.Run(async () => Chart = ChartFactory.CreateLineChart((await _powerPriceRepository.GetPowerPrice(date)).ToList()));
            var powerUsageLoad = Task.Run(async () => DailyChart = ChartFactory.CreatePointedChart((await _powerRepository.GetDaysPowerUsage(date)).ToList(), false, false));

            await priceChartLoad;
            await powerUsageLoad;

            NotifyPropertyChanged(nameof(Chart));
            NotifyPropertyChanged(nameof(DailyChart));
        }
    }
}
