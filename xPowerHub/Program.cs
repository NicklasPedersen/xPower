using xPowerHub.DataStore;

namespace xPowerHub;

public class Program
{
    static readonly string file = @"..\..\..\data\data.json";

    public static void Maind()
    {
        var d = new DAL(@"..\..\..\data\database.db");
        d.DropCreatePopulate();
        foreach (var k in d.GetAllDevices().Result)
        {
            Console.WriteLine(k);
        }
    }

    public static void Main()
    {
        var c = new CRUD(new DAL(@"..\..\..\data\database.db"));
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
                    c.PrintAllDevices();
                    break;
                case ConsoleKey.D1:
                    c.GetDeviceState();
                    break;
                case ConsoleKey.D2:
                    c.SetDeviceState();
                    break;
                case ConsoleKey.D3:
                    c.SwitchAllStates();
                    break;
                case ConsoleKey.D4:
                    c.AddDevice();
                    break;
                case ConsoleKey.D5:
                    c.DeleteDevice();
                    break;
                case ConsoleKey.Escape:
                    break;
                default:
                    Console.WriteLine("invalid keypress");
                    break;
            }
        } while (keypress != ConsoleKey.Escape);
    }
}