using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace xPowerPhoneApp.Test
{
    internal class UAT6 : BaseAndroidTest
    {
        public UAT6(Platform platform) : base(platform)
        {
        }
        [Test]
        public void Add_Hub()
        {
            NavigateToPage();
            AddSmartHub();
            Assert.Pass("Successfull");
        }
        private void NavigateToPage()
        {
            app.Tap("automationAddDeviceButton");
            app.WaitForElement("automationAddHubButton", timeout: TimeSpan.FromSeconds(30));
            app.Tap("automationAddHubButton");
        }
        private void AddSmartHub()
        {
            app.Tap("automationDeviceTypePicker");
            app.WaitForElement("SmartThings", timeout: TimeSpan.FromSeconds(30));
            app.Tap("SmartThings");
            app.EnterText("automationUserKey", "7c3da2a4-cf3f-428f-a5b8-3f9cc4ba76b2");
            app.Tap("automationSearch");
            app.WaitForElement("automationAddHub", timeout: TimeSpan.FromSeconds(30));
            app.Tap("automationAddHub");
            app.WaitForElement(l => l.Marked("automationCheckImage"), timeout: TimeSpan.FromSeconds(30));
        }
    }
}
