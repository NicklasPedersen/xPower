using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace xPowerPhoneApp.Test
{
    internal class UAT4 : BaseAndroidTest
    {
        public UAT4(Platform platform) : base(platform)
        {
        }
        [Test]
        public void See_Status_Of_Devices()
        {
            this.NavigateToPage();
            app.WaitForElement(l => l.Marked("automationSeeUnits"), timeout: TimeSpan.FromSeconds(30));
            app.WaitForElement(l => l.Marked("automationSeeSingleUnit").Switch(), timeout: TimeSpan.FromSeconds(30));
            Assert.Pass("Successfull");
        }
        private void NavigateToPage()
        {
            app.Tap(c => c.Marked("automationSeeUnitsButton"));
        }
    }
}
