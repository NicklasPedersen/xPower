using System;
using System.Collections.Generic;
using System.Text;

namespace xPowerPhoneApp.Models
{
    public class AddDevice : Device
    {
        public bool Adding { get; set; }
        public bool NotAdding { get => !Adding && !Added; }
        public bool Added { get; set; }

        public AddDevice()
        {

        }

        public AddDevice(string name, string mac, bool added = false)
        {
            Name = name;
            Id = mac;
            Added = added;
        }
    }
}
