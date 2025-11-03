namespace WaruSmart.API.Crops.Domain.Model.Commands;

public record UpdateStatusDeviceCommand(
    string Status,
    bool ManuallyActive,
    bool OverwriteAutomation
    );