using System;
using Xamarin.Forms;
using xPowerPhoneApp.Factorys;
using xPowerPhoneApp.Pages;

namespace xPowerPhoneApp
{
    public partial class App : Application
    {
        public App()
        {
            RepositoryFactory.UseMock = true;
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
