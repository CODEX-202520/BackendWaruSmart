using WaruSmart.API.Analytics.Domain.Model;
using WaruSmart.API.Analytics.Domain.Repositories;
using WaruSmart.API.Resources.Domain.Model;
using Microsoft.EntityFrameworkCore;
using WaruSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace WaruSmart.API.Analytics.Infrastructure.Persistence;

public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly AppDbContext _context;
    public AnalyticsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AnalyticsResult>> GetZoneAveragesAsync()
    {
        return await _context.Set<IoTData>()
            .GroupBy(d => d.Zone)
            .Select(g => new AnalyticsResult
            {
                Zone = g.Key,
                AvgTemperature = g.Average(x => x.TemperatureValue ?? 0),
                AvgHumidity = g.Average(x => x.Humidity ?? 0),
                AvgSoilMoisture = g.Average(x => x.SoilMoistureValue ?? 0),
                Count = g.Count()
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<DeviceCountResult>> GetDeviceCountsAsync()
    {
        return await _context.Set<IoTData>()
            .GroupBy(d => d.DeviceIdValue)
            .Select(g => new DeviceCountResult
            {
                DeviceIdValue = g.Key,
                Count = g.Count()
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<DeviceLastValueResult>> GetDeviceLastValuesAsync()
    {
        // Obtener los últimos registros por DeviceIdValue
        var lastValues = await _context.Set<IoTData>()
            .GroupBy(d => d.DeviceIdValue)
            .Select(g => g.OrderByDescending(x => x.Timestamp).FirstOrDefault())
            .ToListAsync();

        return lastValues
            .Where(x => x != null)
            .Select(x => new DeviceLastValueResult
            {
                DeviceIdValue = x.DeviceIdValue,
                TemperatureValue = x.TemperatureValue,
                Humidity = x.Humidity,
                SoilMoistureValue = x.SoilMoistureValue,
                Timestamp = x.Timestamp
            })
            .ToList();
    }

    public async Task<IEnumerable<HistoryResult>> GetHistoryAsync(string zone, string deviceId, DateTime? from, DateTime? to)
    {
        var query = _context.Set<IoTData>().AsQueryable();
        if (!string.IsNullOrEmpty(zone))
            query = query.Where(x => x.Zone == zone);
        if (!string.IsNullOrEmpty(deviceId))
            query = query.Where(x => x.DeviceIdValue == deviceId);
        if (from.HasValue)
            query = query.Where(x => x.Timestamp >= from);
        if (to.HasValue)
            query = query.Where(x => x.Timestamp <= to);
        return await query.Select(x => new HistoryResult
        {
            DeviceIdValue = x.DeviceIdValue,
            Zone = x.Zone,
            Timestamp = x.Timestamp ?? DateTime.MinValue,
            TemperatureValue = x.TemperatureValue,
            Humidity = x.Humidity,
            SoilMoistureValue = x.SoilMoistureValue
        }).ToListAsync();
    }

    public async Task<IEnumerable<TemperatureDistributionResult>> GetTemperatureDistributionAsync(string zone)
    {
        var query = _context.Set<IoTData>().AsQueryable();
        if (!string.IsNullOrEmpty(zone))
            query = query.Where(x => x.Zone == zone);
        var bins = Enumerable.Range(-10, 60).Where(x => x % 5 == 0).ToList();
        var data = await query.Where(x => x.TemperatureValue.HasValue)
            .Select(x => new { x.Zone, Temp = x.TemperatureValue.Value })
            .ToListAsync();
        var result = bins.Select(bin => new TemperatureDistributionResult
        {
            Zone = zone,
            RangeStart = bin,
            RangeEnd = bin + 4,
            Count = data.Count(x => x.Temp >= bin && x.Temp < bin + 5)
        }).Where(x => x.Count > 0).ToList();
        return result;
    }

    public async Task<IEnumerable<DailyAverageResult>> GetDailyAveragesAsync(string zone, int days)
    {
        var from = DateTime.UtcNow.Date.AddDays(-days);
        var query = _context.Set<IoTData>().AsQueryable();
        if (!string.IsNullOrEmpty(zone))
            query = query.Where(x => x.Zone == zone);
        query = query.Where(x => x.Timestamp >= from);
        var result = await query
            .GroupBy(x => new { x.Zone, Date = x.Timestamp.Value.Date })
            .Select(g => new DailyAverageResult
            {
                Zone = g.Key.Zone,
                Date = g.Key.Date,
                AvgTemperature = g.Average(x => x.TemperatureValue ?? 0),
                AvgHumidity = g.Average(x => x.Humidity ?? 0),
                AvgSoilMoisture = g.Average(x => x.SoilMoistureValue ?? 0)
            })
            .OrderBy(x => x.Date)
            .ToListAsync();
        return result;
    }

    public async Task<DeviceStatusResult> GetDeviceStatusAsync()
    {
        var since = DateTime.UtcNow.AddHours(-24);
        var allDevices = await _context.Set<IoTData>().Select(x => x.DeviceIdValue).Distinct().ToListAsync();
        var activeDevices = await _context.Set<IoTData>()
            .Where(x => x.Timestamp >= since)
            .Select(x => x.DeviceIdValue)
            .Distinct()
            .ToListAsync();
        int total = allDevices.Count;
        int active = activeDevices.Count;
        int inactive = total - active;
        double percent = total > 0 ? (active * 100.0) / total : 0;
        return new DeviceStatusResult
        {
            ActiveDevices = active,
            InactiveDevices = inactive,
            TotalDevices = total,
            ActivePercentage = percent
        };
    }
}
