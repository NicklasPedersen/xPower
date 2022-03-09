using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace xPowerPhoneApp.Test
{
    internal class UAT7 : BaseAndroidTest
    {
        public UAT7(Platform platform) : base(platform)
        {
        }

        [Test]
        public void Show_ConnectedDevices()
        {
            this.NavigateToSide();
            Assert.Pass("Sussecfull");
        }

        private void NavigateToSide()
        {
            app.Tap(c => c.Marked("automationSeeUnitsButton"));
            app.WaitForElement(l => l.Marked("automationDeviceNameLabel"), timeout: TimeSpan.FromSeconds(30));
        }
    }
}
