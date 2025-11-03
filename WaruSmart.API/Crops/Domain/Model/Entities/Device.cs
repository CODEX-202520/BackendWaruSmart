using WaruSmart.API.Crops.Domain.Model.Aggregates;
using WaruSmart.API.Crops.Domain.Model.Commands;
using WaruSmart.API.Crops.Domain.Model.ValueObjects;
using WaruSmart.API.Resources.Domain.Model;

namespace WaruSmart.API.Crops.Domain.Model.Entities;

public class Device
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public EDeviceType DeviceType { get;  set; }
    public string DeviceId { get; private set; }
    public string Status { get; private set; } //TODO: This should be an enum
    public DateTime? LastSyncDate { get; set; }
    public string? Location { get; private set; }
    
    public Sowing Sowing { get; set; }
    
    public int SowingId { get; set; }
    
    public double? Humidity { get; set; }
    
    public double? Temprature { get; set; }
    
    public double? SoilMoisture { get; set; }
    
    // Column for manual control of device
    public bool ManuallyActive { get; set; } = false;
    
    // Column to determine if automation should be overridden
    public bool OverwriteAutomation { get; set; } = false;

    public Device(string name, string deviceId, string location)
    {
        Name = name;
        DeviceId = deviceId;
        Location = location;
        Status = "Inactive";
        LastSyncDate = DateTime.MinValue;
    }
    
    public Device()
    {
        Name = "";
        DeviceId = Guid.NewGuid().ToString();
        Status = "Inactive";
        LastSyncDate = null;
    }

    public Device(CreateDeviceCommand command, Sowing sowing)
    {
        DeviceId = command.DeviceId;
        Name = string.Concat(command.DeviceType, '-', DeviceId );
        DeviceType = (EDeviceType)Enum.Parse(typeof(EDeviceType), command.DeviceType);
        Location = command.location;
        Status = command.status;
        LastSyncDate = DateTime.Now;
        SowingId = command.sowingId;
        Sowing = sowing;
        Humidity = 0;
        Temprature = 0;
        SoilMoisture = 0;
        ManuallyActive = false;
        OverwriteAutomation = false;
    }

    public void UpdateStatus(UpdateStatusDeviceCommand command)
    {
        Status = command.Status;
        ManuallyActive = command.ManuallyActive;
        OverwriteAutomation = command.OverwriteAutomation;
    }

    public void UpdateLastSyncDate(DateTime syncDate)
    {
        LastSyncDate = syncDate;
    }

    public void UpdateGeolocation(string geolocation)
    {
        Location = geolocation;
    }
    
    public void UpdateSensorData(IoTData ioTData)
    {
        Humidity = ioTData.Humidity;
        Temprature = ioTData.TemperatureValue;
        SoilMoisture = ioTData.SoilMoistureValue;
        Location = ioTData.Zone;
        LastSyncDate = ioTData.Timestamp;
    }
}