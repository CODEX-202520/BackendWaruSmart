using System;
using WaruSmart.API.Crops.Domain.Model.Aggregates;

namespace WaruSmart.API.Crops.Domain.Model.Aggregates
{
    public class AuditTrail
    {
        // Identidad del agregado
        public int Id { get; private set; }
        
        // Propiedades del agregado
        public int SowingId { get; private set; }
        public string Description { get; private set; }
        public decimal SoilMoisture { get; private set; }
        public decimal SoilTemperature { get; private set; }
        public decimal AirTemperature { get; private set; }
        public decimal AirHumidity { get; private set; }
        public byte[] ImageData { get; private set; }
        public string ImageMimeType { get; private set; }
        public string PhenologicalPhase { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // Navegación a otros agregados
        public Sowing Sowing { get; private set; }

        // Constructor privado para EF Core
        private AuditTrail() { }

        // Constructor para crear un nuevo registro de auditoría
        public static AuditTrail Create(
            int sowingId,
            string description,
            decimal soilMoisture,
            decimal soilTemperature,
            decimal airTemperature,
            decimal airHumidity,
            byte[] imageData,
            string imageMimeType,
            string phenologicalPhase)
        {
            return new AuditTrail
            {
                SowingId = sowingId,
                Description = description,
                SoilMoisture = soilMoisture,
                SoilTemperature = soilTemperature,
                AirTemperature = airTemperature,
                AirHumidity = airHumidity,
                ImageData = imageData,
                ImageMimeType = imageMimeType,
                PhenologicalPhase = phenologicalPhase,
                CreatedAt = DateTime.UtcNow
            };
        }

        // Métodos de dominio
        public void UpdateEnvironmentalData(
            decimal soilMoisture,
            decimal soilTemperature,
            decimal airTemperature,
            decimal airHumidity)
        {
            SoilMoisture = soilMoisture;
            SoilTemperature = soilTemperature;
            AirTemperature = airTemperature;
            AirHumidity = airHumidity;
        }

        public void UpdateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty");
            
            Description = description;
        }

        public void AttachImage(byte[] imageData, string mimeType)
        {
            if (imageData == null || imageData.Length == 0)
                throw new ArgumentException("Image data cannot be empty");
            
            if (string.IsNullOrWhiteSpace(mimeType))
                throw new ArgumentException("Image MIME type cannot be empty");

            ImageData = imageData;
            ImageMimeType = mimeType;
        }

        public void UpdatePhenologicalPhase(string phenologicalPhase)
        {
            if (string.IsNullOrWhiteSpace(phenologicalPhase))
                throw new ArgumentException("Phenological phase cannot be empty");
            
            PhenologicalPhase = phenologicalPhase;
        }
    }
}