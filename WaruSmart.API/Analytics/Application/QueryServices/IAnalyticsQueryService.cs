using WaruSmart.API.Analytics.Domain.Model;

namespace WaruSmart.API.Analytics.Application.QueryServices;

public interface IAnalyticsQueryService
{
    Task<IEnumerable<AnalyticsResult>> GetZoneAveragesAsync();
    Task<IEnumerable<DeviceCountResult>> GetDeviceCountsAsync();
    Task<IEnumerable<DeviceLastValueResult>> GetDeviceLastValuesAsync();
    Task<IEnumerable<HistoryResult>> GetHistoryAsync(string? zone, string? deviceId, DateTime? from, DateTime? to);
    Task<IEnumerable<TemperatureDistributionResult>> GetTemperatureDistributionAsync(string? zone);
    Task<IEnumerable<DailyAverageResult>> GetDailyAveragesAsync(string? zone, int days);
    Task<DeviceStatusResult> GetDeviceStatusAsync();
}
