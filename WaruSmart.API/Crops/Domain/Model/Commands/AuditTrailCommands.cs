namespace WaruSmart.API.Crops.Domain.Model.Commands;

public record CreateAuditTrailCommand(
    int SowingId,
    string Description,
    decimal SoilMoisture,
    decimal SoilTemperature,
    decimal AirTemperature,
    decimal AirHumidity,
    string ImageUrl);

public record GenerateAuditReportCommand(int SowingId);