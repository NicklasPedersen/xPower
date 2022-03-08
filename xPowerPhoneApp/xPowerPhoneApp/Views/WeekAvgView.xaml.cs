using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xPowerPhoneApp.Repositorys.Interfaces;
using xPowerPhoneApp.ViewModels;

namespace xPowerPhoneApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeekAvgView : ContentView
    {
        public WeekAvgView(IPowerRepository repo)
        {
            InitializeComponent();
            BindingContext = new WeekAvgViewModel(repo);
        }

    }
}