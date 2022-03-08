using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xPowerPhoneApp.Interfaces;
using xPowerPhoneApp.ViewModels;

namespace xPowerPhoneApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceListPage : ContentPage, IChangePage
    {
        public DeviceListPage()
        {
            InitializeComponent();
            BindingContext = new DeviceListViewModel(this);
        }

        protected override void OnAppearing()
        {
            _ = ((DeviceListViewModel)BindingContext).InitializeAsync();
            base.OnAppearing();
        }
        /// <summary>
        /// Changes the page to the given page
        /// </summary>
        /// <param name="page">the new page</param>
        public void PushPage(Page page)
        {
            Device.BeginInvokeOnMainThread(() =>
            Navigation.PushAsync(page)
            );
        }

        /// <summary>
        /// Changes to the last page
        /// </summary>
        public void PopPage()
        {
            Device.BeginInvokeOnMainThread(() =>
                Navigation.PopAsync()
            );
        }

        /// <summary>
        /// Pops the last page and pushes a new page
        /// </summary>
        /// <param name="page">The new page to push</param>
        public void PopPushPage(Page page)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PopAsync(false);
                Navigation.PushAsync(page, false);
            }
            );
        }
    }
}