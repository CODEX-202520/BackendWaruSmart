namespace WaruSmart.API.Analytics.Domain.Model;

public class AnalyticsResult
{
    public string Zone { get; set; }
    public double AvgTemperature { get; set; }
    public double AvgHumidity { get; set; }
    public double AvgSoilMoisture { get; set; }
    public int Count { get; set; }
}

public class DeviceCountResult
{
    public string DeviceIdValue { get; set; }
    public int Count { get; set; }
}

public class DeviceLastValueResult
{
    public string DeviceIdValue { get; set; }
    public double? TemperatureValue { get; set; }
    public double? Humidity { get; set; }
    public double? SoilMoistureValue { get; set; }
    public DateTime? Timestamp { get; set; }
}

public class HistoryResult
{
    public string DeviceIdValue { get; set; }
    public string Zone { get; set; }
    public DateTime Timestamp { get; set; }
    public double? TemperatureValue { get; set; }
    public double? Humidity { get; set; }
    public double? SoilMoistureValue { get; set; }
}

public class TemperatureDistributionResult
{
    public string Zone { get; set; }
    public int RangeStart { get; set; }
    public int RangeEnd { get; set; }
    public int Count { get; set; }
}

public class DailyAverageResult
{
    public string Zone { get; set; }
    public DateTime Date { get; set; }
    public double AvgTemperature { get; set; }
    public double AvgHumidity { get; set; }
    public double AvgSoilMoisture { get; set; }
}

public class DeviceStatusResult
{
    public int ActiveDevices { get; set; }
    public int InactiveDevices { get; set; }
    public int TotalDevices { get; set; }
    public double ActivePercentage { get; set; }
}
