using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using xPowerPhoneApp.Interfaces;
using xPowerPhoneApp.Pages;

namespace xPowerPhoneApp.ViewModels
{
    internal class AddDeviceViewModel : PageViewModel
    {
        public ICommand GoToConnectUnit { get; set; }
        public ICommand GoToConnectHub { get; set; }

        public AddDeviceViewModel(IChangePage pageChanger) : base(pageChanger)
        {
            GoToConnectUnit = new Command(() => _pageChanger.PushPage(new ConnectUnitPage()));
            GoToConnectHub = new Command(() => _pageChanger.PushPage(new ConnectHubPage()));
        }
    }
}
