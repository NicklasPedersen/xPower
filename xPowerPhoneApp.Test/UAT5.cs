using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace xPowerPhoneApp.Test
{
    internal class UAT5 : BaseAndroidTest
    {
        public UAT5(Platform platform) : base(platform)
        {
        }

        [Test]
        public void Create_WithValidCredentials_ShouldCreateUser()
        {
            this.NavigateToSide();
            this.AddUnit();
            Assert.Pass("Unit have been Added Sussecfull");
        }

        private void NavigateToSide()
        {
            app.Tap(c => c.Marked("automationAddDeviceButton"));
            app.WaitForElement(l => l.Marked("automationAddUnitsButton"), timeout: TimeSpan.FromSeconds(30));
            app.Tap(c => c.Marked("automationAddUnitsButton"));
            app.WaitForElement(l => l.Text("Søger"), timeout: TimeSpan.FromSeconds(30));
        }

        private void AddUnit()
        {
            app.WaitForElement(l => l.Marked("automationAddUnitButton"), timeout: TimeSpan.FromSeconds(30));
            app.Tap(c => c.Marked("automationAddUnitButton"));
            app.WaitForElement(l => l.Marked("automationCheckImage"), timeout: TimeSpan.FromSeconds(30));
        }
    }
}
