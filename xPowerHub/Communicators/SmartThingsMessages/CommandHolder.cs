﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace xPowerHub.Communicators.SmartThingsMessages
{
    internal class CommandHolder
    {
        [JsonPropertyName("commands")]
        public Command[] Commands { get; set; }
    }
}
