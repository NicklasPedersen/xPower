using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace xPowerPhoneApp.Test
{
    [TestFixture(Platform.Android)]
    internal class UAT5AddUnit
    {
        IApp app;
        Platform platform;

        public string _preSetUnit;


        public UAT5AddUnit(Platform platform)
        {
            _preSetUnit = "btotest1";
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
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
            app.WaitForElement(l => l.Text("automationMac"), timeout: TimeSpan.FromSeconds(30));
            app.Tap(c => c.Text("automationMac").Sibling().Marked("automationAddUnitButton"));
            app.WaitForElement(l => l.Text("automationMac").Sibling().Marked("automationCheckImage"), timeout: TimeSpan.FromSeconds(30));
        }
    }
}
