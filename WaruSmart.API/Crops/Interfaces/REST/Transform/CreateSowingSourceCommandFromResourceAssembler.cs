using WaruSmart.API.Crops.Domain.Model.Commands;
using WaruSmart.API.Crops.Interfaces.REST.Resources;

namespace WaruSmart.API.Crops.Interfaces.REST.Transform;

public static class CreateSowingSourceCommandFromResourceAssembler
{
    public static CreateSowingCommand ToCommandFromResource(CreateSowingResource resource)
    {
        return new CreateSowingCommand(
            resource.AreaLand,
            resource.CropId,
            resource.UserId,
            resource.PhenologicalPhase);
    }
}