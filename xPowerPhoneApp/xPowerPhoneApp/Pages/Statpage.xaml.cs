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
    public partial class Statpage : ContentPage, IChangePage
    {
        public Statpage()
        {
            InitializeComponent();
            BindingContext = new StatViewModel(this);
        }

        protected override void OnAppearing()
        {
            ((StatViewModel)BindingContext).StartGettingData();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            ((StatViewModel)BindingContext).StopGettingData();
            base.OnDisappearing();
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