using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xPowerHub;

internal interface ISmart
{
    // sets the state to true and returns whether or not it was successful
    public bool TurnOn();
    // sets the state to false and returns whether or not it was successful
    public bool TurnOff();
    // returns the current state or null if error
    public bool? GetCurrentState();
    // sets the state and returns whether or not it was successful
    public bool SetState(bool state);
}
