using System;
using System.Collections.Generic;
using System.Text;

namespace xPowerPhoneApp.Models
{
    public class AddDevice
    {
        public string Name { get; set; }
        public string Mac { get; set; }
        public bool Adding { get; set; }
        public bool NotAdding { get => !Adding && !Added; }
        public bool Added { get; set; }

        public AddDevice()
        {

        }

        public AddDevice(string name, string mac, bool added = false)
        {
            Name = name;
            Mac = mac;
            Added = added;
        }
    }
}
