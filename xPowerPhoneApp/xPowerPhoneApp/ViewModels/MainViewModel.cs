using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using xPowerPhoneApp.Interfaces;
using xPowerPhoneApp.Pages;

namespace xPowerPhoneApp.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {

        public ICommand GoToAddDevice { get; set; }
        public MainViewModel(IChangePage pageChanger) : base(pageChanger)
        {

            GoToAddDevice = new Command(() => _pageChanger.PushPage(new AddDevicePage()));
        }
    }
}
