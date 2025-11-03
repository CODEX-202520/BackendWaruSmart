using WaruSmart.API.Crops.Domain.Model.Commands;
using WaruSmart.API.Crops.Interfaces.REST.Resources;

namespace WaruSmart.API.Crops.Interfaces.REST.Transform;

public static class UpdateStatusDeviceCommandFromResourceAssembler
{
    public static UpdateStatusDeviceCommand ToCommandFromResource(UpdateStatusDeviceResource resource)
    {
        return new UpdateStatusDeviceCommand(
            resource.Status,
            resource.ManuallyActive,
            resource.OverwriteAutomation
        );
    }
}