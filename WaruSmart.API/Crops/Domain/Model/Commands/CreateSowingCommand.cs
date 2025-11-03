using WaruSmart.API.Crops.Domain.Model.ValueObjects;

namespace WaruSmart.API.Crops.Domain.Model.Commands;

public record CreateSowingCommand(
    int AreaLand,
    int CropId,
    int UserId,
    EPhenologicalPhase? PhenologicalPhase = null);