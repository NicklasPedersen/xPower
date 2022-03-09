using xPowerHub.Models;

namespace xPowerHub.DataStore;

public interface IDataStore<T>
{
    void AddTable();
    void RemoveTable();
    Task<bool> SaveAsync(T item);
    Task<bool> UpdateAsync(T item);
    Task<T> GetAsync(string key);
    Task<IEnumerable<T>> GetAllAsync(bool forceRefresh = false);
}

public interface IDataStorePower : IDataStore<PowerUsage>
{
    Task<PowerUsage> GetAsync(DateTime key);
    Task<IEnumerable<PowerUsage>> GetPowerUsageWeekdayAvgAsync();
    Task<IEnumerable<PowerUsage>> GetPowerUsageHourlyAvgAsync(DateTime date);
}