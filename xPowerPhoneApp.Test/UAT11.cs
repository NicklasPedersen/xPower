using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace xPowerPhoneApp.Test
{
    internal class UAT11 : BaseAndroidTest
    {
        private string _name;
        public UAT11(Platform platform) : base(platform)
        {
            _name = "Naming test";
        }


        [Test]
        public void Update_ChangeNameOnDevice_ShouldChange()
        {
            this.NavigateToSide();
            this.InputName();
            Assert.Pass("Unit have been Added Sussecfull");
        }

        private void NavigateToSide()
        {
            app.Tap(c => c.Marked("automationSeeUnitsButton"));
            app.WaitForElement(l => l.Marked("automationDeviceNameLabel"), timeout: TimeSpan.FromSeconds(30));
            app.WaitForElement(l => l.Marked("automationEditButton"), timeout: TimeSpan.FromSeconds(30));
            app.Tap(c => c.Marked("automationEditButton"));
        }

        private void InputName()
        {
            app.WaitForElement(l => l.Marked("automationNameEntry"), timeout: TimeSpan.FromSeconds(30));
            app.EnterText(e => e.Marked("automationNameEntry"), _name);
            app.Tap(c => c.Marked("automationEditNameButton"));
            app.Tap(c => c.Marked("automationEditNameButton"));
            app.WaitForElement(l => l.Marked("automationDeviceNameLabel"), timeout: TimeSpan.FromSeconds(30));
        }
    }
}
