using System;
using System.Collections.Generic;
using System.Text;

namespace xPowerPhoneApp.Models
{
    internal class AddDevice : Device
    {
        public bool Adding { get; set; }
        public bool NotAdding { get => !Adding && !Added; }
        public bool Added { get; set; }

        public AddDevice()
        {

        }

        public AddDevice(Device device, bool added = false)
        {
            Name = device.Name;
            Id = device.Id;
            Ip = device.Ip;
            ParentId = device.ParentId;
            Added = added;
        }
    }
}
