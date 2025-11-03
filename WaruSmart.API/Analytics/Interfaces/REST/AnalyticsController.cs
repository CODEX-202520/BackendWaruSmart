using Microsoft.AspNetCore.Mvc;
using WaruSmart.API.Analytics.Application.QueryServices;
using WaruSmart.API.Analytics.Domain.Model;

namespace WaruSmart.API.Analytics.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsQueryService _analyticsQueryService;
    public AnalyticsController(IAnalyticsQueryService analyticsQueryService)
    {
        _analyticsQueryService = analyticsQueryService;
    }

    [HttpGet("zone-averages")]
    public async Task<ActionResult<IEnumerable<AnalyticsResult>>> GetZoneAverages()
    {
        var result = await _analyticsQueryService.GetZoneAveragesAsync();
        return Ok(result);
    }

    [HttpGet("device-counts")]
    public async Task<ActionResult<IEnumerable<DeviceCountResult>>> GetDeviceCounts()
    {
        var result = await _analyticsQueryService.GetDeviceCountsAsync();
        return Ok(result);
    }

    [HttpGet("device-last-values")]
    public async Task<ActionResult<IEnumerable<DeviceLastValueResult>>> GetDeviceLastValues()
    {
        var result = await _analyticsQueryService.GetDeviceLastValuesAsync();
        return Ok(result);
    }

    [HttpGet("history")]
    public async Task<ActionResult<IEnumerable<HistoryResult>>> GetHistory([FromQuery] string? zone, [FromQuery] string? deviceId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var result = await _analyticsQueryService.GetHistoryAsync(zone, deviceId, from, to);
        return Ok(result);
    }

    [HttpGet("temperature-distribution")]
    public async Task<ActionResult<IEnumerable<TemperatureDistributionResult>>> GetTemperatureDistribution([FromQuery] string? zone)
    {
        var result = await _analyticsQueryService.GetTemperatureDistributionAsync(zone);
        return Ok(result);
    }

    [HttpGet("daily-averages")]
    public async Task<ActionResult<IEnumerable<DailyAverageResult>>> GetDailyAverages([FromQuery] string? zone, [FromQuery] int days = 7)
    {
        var result = await _analyticsQueryService.GetDailyAveragesAsync(zone, days);
        return Ok(result);
    }

    [HttpGet("device-status")]
    public async Task<ActionResult<DeviceStatusResult>> GetDeviceStatus()
    {
        var result = await _analyticsQueryService.GetDeviceStatusAsync();
        return Ok(result);
    }
}
