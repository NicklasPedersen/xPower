using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest;

namespace xPowerPhoneApp.Test
{
    internal class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                //return ConfigureApp.Android.StartApp();
                return ConfigureApp.Android.InstalledApp("com.companyname.xPowerPhoneApp").EnableLocalScreenshots().StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}
