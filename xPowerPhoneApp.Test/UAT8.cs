using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace xPowerPhoneApp.Test
{
    internal class UAT8 : BaseAndroidTest
    {
        public UAT8(Platform platform) : base(platform)
        {
        }

        [Test]
        public void Show_PriceVsUsage_ShouldShow()
        {
            this.NavigateToSide();
            Assert.Pass("Sussecfull");
        }

        private void NavigateToSide()
        {
            app.Tap(c => c.Marked("automationSeeStatButton"));
            app.WaitForElement(l => l.Marked("automationPriceButton"), timeout: TimeSpan.FromSeconds(30));
            app.Tap(c => c.Marked("automationPriceButton"));
            app.WaitForElement(l => l.Marked("automationPriceChartView"), timeout: TimeSpan.FromSeconds(30));
        }
    }
}
