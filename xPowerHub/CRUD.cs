using xPowerHub.Communicators;
using xPowerHub.DataStore;

namespace xPowerHub;

internal class CRUD
{
    readonly IDataStore _store;
    public CRUD(IDataStore store)
    {
        _store = store;
    }
    bool GetDevNum(out int devnum)
    {
        var k = Console.ReadLine();
        if (int.TryParse(k, out devnum))
        {
            return 0 <= devnum && devnum < _store.GetAllDevices().Result.Count();
        }
        return false;
    }
    bool GetBool(out bool b)
    {
        var k = Console.ReadLine();
        return bool.TryParse(k, out b);
    }
    
    int MaxDevices()
    {
        return GetAllDevices().Length;
    }

    ISmart? GetDevice(int id)
    {
        if (0 <= id && id < MaxDevices())
        {
            return GetAllDevices()[id];
        }
        return null;
    }

    ISmart[] GetAllDevices()
    {
        return _store.GetAllDevices().Result.ToArray();
    }

    public void PrintAllDevices()
    {
        for (var i = 0; i < _store.GetAllDevices().Result.Count(); i++)
        {
            Console.WriteLine(i + ": " + GetDevice(i));
        }
    }
    public void HandleKeyPresses()
    {
        ConsoleKey keypress;
        do
        {
            Console.WriteLine("Select option");
            Console.WriteLine("0: print all current devices");
            Console.WriteLine("1: get status of device");
            Console.WriteLine("2: set status of device");
            Console.WriteLine("3: flip all devices");
            Console.WriteLine("4: add a device");
            Console.WriteLine("5: delete a device");
            Console.WriteLine("esc: exit");
            keypress = Console.ReadKey().Key;
            Console.WriteLine();
            switch (keypress)
            {
                case ConsoleKey.D0:
                    PrintAllDevices();
                    break;
                case ConsoleKey.D1:
                    GetDeviceState();
                    break;
                case ConsoleKey.D2:
                    SetDeviceState();
                    break;
                case ConsoleKey.D3:
                    SwitchAllStates();
                    break;
                case ConsoleKey.D4:
                    AddDevice();
                    break;
                case ConsoleKey.D5:
                    DeleteDevice();
                    break;
                case ConsoleKey.Escape:
                    break;
                default:
                    Console.WriteLine("invalid keypress");
                    break;
            }
        } while (keypress != ConsoleKey.Escape);
    }
    public void GetDeviceState()
    {
        Console.WriteLine("status of what device number?");
        if (GetDevNum(out int deviceNumber))
        {
            var response = GetDevice(deviceNumber)!.GetCurrentState();
            Console.WriteLine("state of " + deviceNumber + " is: " + response);
        }
        else
        {
            Console.WriteLine("device does not exist");
        }
    }

    public void SetDeviceState()
    {
        Console.WriteLine("what device number?");
        if (GetDevNum(out int deviceNumber))
        {
            Console.WriteLine("true or false");
            if (GetBool(out bool desiredState))
            {
                if (GetDevice(deviceNumber)!.SetState(desiredState))
                {
                    Console.WriteLine("success");
                }
                else
                {
                    Console.WriteLine("errornous response");
                }
            }
        }
        else
        {
            Console.WriteLine("device does not exist");
        }
    }

    public void SwitchAllStates()
    {
        foreach (var dev in _store.GetAllDevices().Result.ToArray())
        {
            var current = dev.GetCurrentState();
            if (current is bool b)
            {
                dev.SetState(!b);
            }
            else
            {
                Console.WriteLine("error reading device");
            }
        }
    }

    public void AddDevice()
    {
        _store.AddWizAsync(WizDeviceCommunicator.GetNewDevice());
        Console.WriteLine("success");
    }

    public void DeleteDevice()
    {
        Console.WriteLine("delete what device number?");
        if (GetDevNum(out int deviceNumber))
        {
            var dev = GetDevice(deviceNumber);
            if (dev is WizDevice w)
            {
                _store.DeleteWizAsync(w);
            }
            else if (dev is SmartThingsDevice s)
            {
                _store.DeleteSmartAsync(s);
            }
        }
        else
        {
            Console.WriteLine("device does not exist");
        }
    }
}
