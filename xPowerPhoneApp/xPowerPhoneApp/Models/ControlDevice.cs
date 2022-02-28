using System;
using System.Collections.Generic;
using System.Text;

namespace xPowerPhoneApp.Models
{
    public class ControlDevice : Device
    {
        public bool Status { get; set; }
        public bool IsStatusKnown { get; set; }
    }
}
