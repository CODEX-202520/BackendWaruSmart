namespace WaruSmart.API.Crops.Interfaces.REST.Resources;

public record DeviceResource(
    int Id,
    string Name,
    string DeviceType,
    string DeviceId,
    string Status,
    DateTime? LastSyncDate,
    string Location,
    int SowingId,
    double? Humidity,
    double? Temperature,
    double? SoilMoisture,
    DateTime? TimeSinceLastSync,
    bool ManuallyActive,
    bool OverwriteAutomation
);