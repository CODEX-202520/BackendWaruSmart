namespace WaruSmart.API.Crops.Interfaces.REST.Resources;

public record UpdateStatusDeviceResource(
    string Status,
    bool ManuallyActive,
    bool OverwriteAutomation);