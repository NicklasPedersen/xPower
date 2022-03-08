using xPowerHub.DataStore;

namespace xPowerHub;

public class Program
{
    //static readonly string file = @"..\..\..\data\data.json";

    public static void Main()
    {
        var d = new DAL(@"..\..\..\data\database.db");
        var c = new CRUD(d);
        c.HandleKeyPresses();
    }
}