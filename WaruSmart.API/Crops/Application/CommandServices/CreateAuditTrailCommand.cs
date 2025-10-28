namespace WaruSmart.API.Crops.Application.CommandServices
{
    public class CreateAuditTrailCommand
    {
        public int SowingId { get; set; }
        public string Description { get; set; }
        public decimal SoilMoisture { get; set; }
        public decimal SoilTemperature { get; set; }
        public decimal AirTemperature { get; set; }
        public decimal AirHumidity { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public string PhenologicalPhase { get; set; }
    }
}