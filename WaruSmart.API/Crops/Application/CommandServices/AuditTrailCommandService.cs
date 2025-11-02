using WaruSmart.API.Crops.Domain.Model.Commands;
using WaruSmart.API.Crops.Domain.Model.Aggregates;
using WaruSmart.API.Crops.Domain.Repositories;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.Layout.Properties;

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

            // Agregar estilos al título
            var titleStyle = new Style()
                .SetFontSize(20)
                .SetBold()
                .SetFontColor(new DeviceRgb(0, 95, 64)); // Color verde WaruSmart

            document.Add(new Paragraph($"Reporte de Auditoría para Cultivo {sowing.Id}")
                .AddStyle(titleStyle)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(20));

            document.Add(new Paragraph($"Fecha de Generación: {DateTime.Now:dd/MM/yyyy HH:mm}")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetMarginBottom(30));

            // Crear tabla para la línea de tiempo
            var table = new Table(new float[] { 1, 4 })
                .SetWidth(UnitValue.CreatePercentValue(100))
                .SetMarginBottom(20);

            foreach (var audit in audits)
            {
                // Celda de fecha con línea vertical
                var dateCell = new Cell()
                    .Add(new Paragraph($"{audit.CreatedAt:dd/MM/yyyy\nHH:mm}")
                        .SetTextAlignment(TextAlignment.CENTER))
                    .SetBorderLeft(new SolidBorder(new DeviceRgb(0, 95, 64), 2)) // Línea verde
                    .SetBorderRight(Border.NO_BORDER)
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderBottom(Border.NO_BORDER)
                    .SetPaddingLeft(10)
                    .SetPaddingRight(10);

                // Celda de contenido con fondo
                var contentCell = new Cell()
                    .Add(new Paragraph($"Descripción: {audit.Description}")
                        .SetMarginBottom(5))
                    .Add(new Paragraph()
                        .Add(new Text("Condiciones ambientales:").SetBold())
                        .SetMarginBottom(5))
                    .Add(new Paragraph()
                        .Add($"• Humedad del Suelo: {audit.SoilMoisture}%\n")
                        .Add($"• Temperatura del Suelo: {audit.SoilTemperature}°C\n")
                        .Add($"• Temperatura del Aire: {audit.AirTemperature}°C\n")
                        .Add($"• Humedad del Aire: {audit.AirHumidity}%")
                        .SetMarginLeft(20));

                if (audit.ImageData != null && audit.ImageData.Length > 0)
                {
                    try
                    {
                        using var imageStream = new MemoryStream(audit.ImageData);
                        var imgData = ImageDataFactory.Create(audit.ImageData);
                        var img = new Image(imgData)
                            .SetWidth(150)
                            .SetBorderRadius(new BorderRadius(75)) // Hacer la imagen circular
                            .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                            .SetMarginTop(10)
                            .SetMarginBottom(10);
                        contentCell.Add(img);
                    }
                    catch (Exception)
                    {
                        contentCell.Add(new Paragraph("(Imagen no disponible)"));
                    }
                }

                // Aplicar estilos a la celda de contenido
                contentCell
                    .SetBackgroundColor(new DeviceRgb(245, 247, 250))
                    .SetBorder(new SolidBorder(new DeviceRgb(0, 95, 64), 1))
                    .SetBorderRadius(new BorderRadius(5))
                    .SetPadding(10)
                    .SetMargin(10);

                // Agregar celdas a la tabla
                table.AddCell(dateCell);
                table.AddCell(contentCell);
            }

            // Agregar la tabla al documento
            document.Add(table);

            document.Close();
            return ms.ToArray();
        }
    }
}