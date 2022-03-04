using xPowerHub.Models;

namespace xPowerHub.DataStore;

public interface IDataStore
{
    Task<bool> AddWizAsync(WizDevice item);
    Task<bool> UpdateWizAsync(WizDevice item);
    Task<bool> DeleteWizAsync(WizDevice dev);
    Task<WizDevice?> GetWizAsync(int id);
    Task<IEnumerable<WizDevice>> GetWizsAsync(bool forceRefresh = false);
    Task<bool> AddSmartAsync(SmartThingsDevice item);
    Task<bool> UpdateSmartAsync(SmartThingsDevice item);
    Task<bool> DeleteSmartAsync(SmartThingsDevice dev);
    Task<SmartThingsDevice?> GetSmartAsync(int id);
    Task<SmartThingsDevice?> GetSmartAsync(string id);
    Task<IEnumerable<SmartThingsDevice>> GetSmartsAsync(bool forceRefresh = false);
    Task<IEnumerable<ISmart>> GetAllDevicesAsync(bool forceRefresh = false);
    Task<bool> AddPowerStatementAsync(PowerStatement powerStatement);
    Task<IEnumerable<PowerStatement>> GetPowerStatementWeekdayAvgAsync();
    Task<IEnumerable<PowerStatement>> GetPowerStatementHourlyAvgAsync(DateTime date);
}
