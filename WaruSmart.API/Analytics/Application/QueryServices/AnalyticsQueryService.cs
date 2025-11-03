using WaruSmart.API.Analytics.Domain.Model;
using WaruSmart.API.Analytics.Domain.Repositories;

namespace WaruSmart.API.Analytics.Application.QueryServices;

public class AnalyticsQueryService : IAnalyticsQueryService
{
    private readonly IAnalyticsRepository _analyticsRepository;
    public AnalyticsQueryService(IAnalyticsRepository analyticsRepository)
    {
        _analyticsRepository = analyticsRepository;
    }

    public async Task<IEnumerable<AnalyticsResult>> GetZoneAveragesAsync()
        => await _analyticsRepository.GetZoneAveragesAsync();

    public async Task<IEnumerable<DeviceCountResult>> GetDeviceCountsAsync()
        => await _analyticsRepository.GetDeviceCountsAsync();

    public async Task<IEnumerable<DeviceLastValueResult>> GetDeviceLastValuesAsync()
        => await _analyticsRepository.GetDeviceLastValuesAsync();

    public async Task<IEnumerable<HistoryResult>> GetHistoryAsync(string? zone, string? deviceId, DateTime? from, DateTime? to)
        => await _analyticsRepository.GetHistoryAsync(zone, deviceId, from, to);

    public async Task<IEnumerable<TemperatureDistributionResult>> GetTemperatureDistributionAsync(string? zone)
        => await _analyticsRepository.GetTemperatureDistributionAsync(zone);

    public async Task<IEnumerable<DailyAverageResult>> GetDailyAveragesAsync(string? zone, int days)
        => await _analyticsRepository.GetDailyAveragesAsync(zone, days);

    public async Task<DeviceStatusResult> GetDeviceStatusAsync()
        => await _analyticsRepository.GetDeviceStatusAsync();
}
