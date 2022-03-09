using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace xPowerPhoneApp.Test
{
    internal class UAT1 : BaseAndroidTest
    {
        public UAT1(Platform platform) : base(platform)
        {
        }

        [Test]
        public void Show_CurrentPowerUsage()
        {
            this.NavigateToSide();
            Assert.Pass("Sussecfull");
        }

        private void NavigateToSide()
        {
            app.WaitForElement(l => l.Marked("automationUsage"), timeout: TimeSpan.FromSeconds(30));
        }
    }
}
