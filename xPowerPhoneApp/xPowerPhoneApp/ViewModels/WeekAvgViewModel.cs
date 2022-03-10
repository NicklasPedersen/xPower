using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPowerPhoneApp.Factorys;
using xPowerPhoneApp.Interfaces;
using xPowerPhoneApp.Models;
using xPowerPhoneApp.Repositorys.Interfaces;

namespace xPowerPhoneApp.ViewModels
{
    internal class WeekAvgViewModel : BaseViewModel
    {
        public Chart Chart { get; set; }

        private IPowerRepository _powerRepository;

        public WeekAvgViewModel(IPowerRepository powerRepository) : base()
        {
            _powerRepository = powerRepository;
            _ = SetupAsync();
        }

        private async Task SetupAsync()
        {

            Chart = ChartFactory.CreateLineChart((await _powerRepository.GetWeeklyAvg()).ToList(), true);
            NotifyPropertyChanged(nameof(Chart));
        }
    }
}
