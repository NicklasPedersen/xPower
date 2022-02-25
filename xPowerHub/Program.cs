namespace xPowerHub;

public class Program
{
    static readonly string file = @"..\..\..\data.json";

    static readonly List<ISmart> devices = DeviceSerializer.Deserialize(File.ReadAllText(file)).ToList();

    static bool GetDevNum(out int devnum)
    {
        var k = Console.ReadLine();
        if (int.TryParse(k, out devnum))
        {
            return 0 <= devnum && devnum < devices.Count;
        }
        return false;
    }
    static bool GetBool(out bool b)
    {
        var k = Console.ReadLine();
        return bool.TryParse(k, out b);
    }

    static void PrintAllDevices()
    {
        for (var i = 0; i < devices.Count; i++)
        {
            Console.WriteLine(i + ": " + devices[i]);
        }
    }

    static void GetDeviceState()
    {
        Console.WriteLine("status of what device number?");
        if (GetDevNum(out int devnum))
        {
            var response = devices[devnum].GetCurrentState();
            Console.WriteLine("state of " + devnum + " is: " + response);
        }
        else
        {
            Console.WriteLine("device does not exist");
        }
    }

    public static void SetDeviceState()
    {
        Console.WriteLine("what device number?");
        if (GetDevNum(out int devnus))
        {
            Console.WriteLine("true or false");
            if (GetBool(out bool desiredState))
            {
                if (devices[devnus].SetState(desiredState))
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

    public static void SwitchAllStates()
    {
        foreach (var dev in devices)
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

    public static void AddDevice()
    {
        devices.Add(WizDeviceCommunicator.GetNewDevice());
        Console.WriteLine("success");
    }

    public static void DeleteDevice()
    {
        Console.WriteLine("delete what device number?");
        if (GetDevNum(out int devnu))
        {
            devices.RemoveAt(devnu);
        }
        else
        {
            Console.WriteLine("device does not exist");
        }
    }


    public static void Main()
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
        File.WriteAllText(file, DeviceSerializer.Serialize(devices.ToArray()));
    }
}