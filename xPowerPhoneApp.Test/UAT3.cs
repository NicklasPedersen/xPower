using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace xPowerPhoneApp.Test
{
    internal class UAT3 : BaseAndroidTest
    {
        public UAT3(Platform platform) : base(platform)
        {
        }

        [Test]
        public void Show_WeeklyAvg_ShouldShow()
        {
            this.NavigateToSide();
            Assert.Pass("Sussecfull");
        }

        private void NavigateToSide()
        {
            app.Tap(c => c.Marked("automationSeeStatButton"));
            app.WaitForElement(l => l.Marked("automationWeeklyAvgButton"), timeout: TimeSpan.FromSeconds(30));
            app.Tap(c => c.Marked("automationWeeklyAvgButton"));
            app.WaitForElement(l => l.Marked("automationWeekChartView"), timeout: TimeSpan.FromSeconds(30));
        }
    }
}
