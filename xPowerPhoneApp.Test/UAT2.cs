using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace xPowerPhoneApp.Test
{
    internal class UAT2 : BaseAndroidTest
    {
        public UAT2(Platform platform) : base(platform)
        {
        }
        [Test]
        public void See_Devices()
        {
            this.NavigateToSide();
            app.WaitForElement(l => l.Marked("automationSeeUnits"), timeout: TimeSpan.FromSeconds(30));
            app.WaitForElement(l => l.Marked("automationSeeSingleUnit"), timeout: TimeSpan.FromSeconds(30));
            Assert.Pass("Successfull");
        }
        private void NavigateToSide()
        {
            app.Tap(c => c.Marked("automationSeeUnitsButton"));
        }
    }
}
