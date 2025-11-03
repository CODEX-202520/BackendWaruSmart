using WaruSmart.API.Crops.Domain.Model.Entities;
using WaruSmart.API.Crops.Interfaces.REST.Resources;

namespace WaruSmart.API.Crops.Interfaces.REST.Transform;

public static class DeviceResourceFromEntityAssembler
{
    public static DeviceResource ToResourceFromEntity(Device device)
    {
        return new DeviceResource(
            device.Id,
            device.Name,
            device.DeviceType.ToString(),
            device.DeviceId,
            device.Status,
            device.LastSyncDate,
            device.Location,
            device.SowingId,
            device.Humidity,
            device.Temprature,
            device.SoilMoisture,
            device.LastSyncDate,
            device.ManuallyActive,
            device.OverwriteAutomation
        );

    }
}