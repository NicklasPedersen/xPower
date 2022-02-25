using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace xPowerPhoneApp.Interfaces
{

    /// <summary>
    /// Can Change the page
    /// </summary>
    internal interface IChangePage
    {
        void PushPage(Page page);
        void PopPushPage(Page page);
        void PopPage();
    }
}
