using WaruSmart.API.Crops.Domain.Model.ValueObjects;

namespace WaruSmart.API.Crops.Interfaces.REST.Resources;

public record CreateSowingResource(
    int AreaLand,
    int CropId,
    int UserId,
    EPhenologicalPhase? PhenologicalPhase = null);