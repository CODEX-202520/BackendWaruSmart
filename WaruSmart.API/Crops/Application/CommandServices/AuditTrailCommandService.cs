using WaruSmart.API.Crops.Domain.Model.Commands;
using WaruSmart.API.Crops.Domain.Model.Aggregates;
using WaruSmart.API.Crops.Domain.Repositories;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;

namespace WaruSmart.API.Crops.Application.CommandServices;

public interface IAuditTrailCommandService
{
    Task<AuditTrail> Handle(CreateAuditTrailCommand command);
    Task<byte[]> Handle(GenerateAuditReportCommand command);
}

public class AuditTrailCommandService : IAuditTrailCommandService
{
    private readonly IAuditTrailRepository _auditTrailRepository;
    private readonly ISowingRepository _sowingRepository;

    public AuditTrailCommandService(IAuditTrailRepository auditTrailRepository, ISowingRepository sowingRepository)
    {
        _auditTrailRepository = auditTrailRepository;
        _sowingRepository = sowingRepository;
    }

    public async Task<AuditTrail> Handle(CreateAuditTrailCommand command)
    {
        var auditTrail = AuditTrail.Create(
            command.SowingId,
            command.Description,
            command.SoilMoisture,
            command.SoilTemperature,
            command.AirTemperature,
            command.AirHumidity,
            command.ImageData,
            command.ImageMimeType,
            "Initial");

        return await _auditTrailRepository.AddAsync(auditTrail);
    }

    public async Task<byte[]> Handle(GenerateAuditReportCommand command)
    {
        var sowing = await _sowingRepository.FindByIdAsync(command.SowingId);
        var audits = await _auditTrailRepository.ListBySowingIdAsync(command.SowingId);

        using (var ms = new MemoryStream())
        {
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            document.Add(new Paragraph($"Reporte de Auditoría para Cultivo {sowing.Id}"));
            document.Add(new Paragraph($"Fecha de Generación: {DateTime.Now:dd/MM/yyyy HH:mm}"));
            
            foreach (var audit in audits)
            {
                document.Add(new Paragraph("-------------------"));
                document.Add(new Paragraph($"Fecha: {audit.CreatedAt:dd/MM/yyyy HH:mm}"));
                document.Add(new Paragraph($"Descripción: {audit.Description}"));
                document.Add(new Paragraph($"Humedad del Suelo: {audit.SoilMoisture}%"));
                document.Add(new Paragraph($"Temperatura del Suelo: {audit.SoilTemperature}°C"));
                document.Add(new Paragraph($"Temperatura del Aire: {audit.AirTemperature}°C"));
                document.Add(new Paragraph($"Humedad del Aire: {audit.AirHumidity}%"));

                if (audit.ImageData != null && audit.ImageData.Length > 0)
                {
                    try
                    {
                        using (var imageStream = new MemoryStream(audit.ImageData))
                        {
                            var imgData = ImageDataFactory.Create(audit.ImageData);
                            var img = new Image(imgData);
                            img.SetWidth(200);
                            document.Add(img);
                        }
                    }
                    catch (Exception)
                    {
                        document.Add(new Paragraph("(Imagen no disponible)"));
                    }
                }
            }

            document.Close();
            return ms.ToArray();
        }
    }
}