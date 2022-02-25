using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace xPowerPhoneApp.CustomItems
{

    /// <summary>
    /// Custom frame that enables corner radius diffent foreach corner
    /// </summary>
    public class CustomFrame : Frame
    {
        public static new readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CustomFrame), typeof(CornerRadius), typeof(CustomFrame));
        public CustomFrame()
        {
            base.CornerRadius = 0;
        }

        public new CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

    }
}