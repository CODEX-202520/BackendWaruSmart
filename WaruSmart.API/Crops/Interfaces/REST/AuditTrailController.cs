using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WaruSmart.API.Crops.Application.CommandServices;
using WaruSmart.API.Crops.Domain.Model.Aggregates;
using WaruSmart.API.Crops.Domain.Model.Commands;
using WaruSmart.API.Crops.Domain.Repositories;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WaruSmart.API.Crops.Interfaces.REST;

[ApiController]
[Route("api/v1/crops/sowing/{sowingId}/audit-trail")]
public class AuditTrailController : ControllerBase
{
    private readonly IAuditTrailCommandService _auditTrailCommandService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IAuditTrailRepository _auditTrailRepository;

    public AuditTrailController(IAuditTrailCommandService auditTrailCommandService, IWebHostEnvironment webHostEnvironment, IAuditTrailRepository auditTrailRepository)
    {
        _auditTrailCommandService = auditTrailCommandService;
        _webHostEnvironment = webHostEnvironment;
        _auditTrailRepository = auditTrailRepository;
    }

    [HttpPost]
    [RequestSizeLimit(10_000_000)] // limit to ~10MB, tune as needed
    public async Task<IActionResult> CreateAudit(
        int sowingId,
        [FromForm] string description,
        [FromForm] decimal soilMoisture,
        [FromForm] decimal soilTemperature,
        [FromForm] decimal airTemperature,
        [FromForm] decimal airHumidity,
        [FromForm] string phenologicalPhase,
        [FromForm] IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            return BadRequest(new { errors = new { image = new[] { "The image is required" } } });
        }

        byte[] imageData;
        using (var memoryStream = new MemoryStream())
        {
            await image.CopyToAsync(memoryStream);
            imageData = memoryStream.ToArray();
        }

        var auditTrail = AuditTrail.Create(
            sowingId: sowingId,
            description: description,
            soilMoisture: soilMoisture,
            soilTemperature: soilTemperature,
            airTemperature: airTemperature,
            airHumidity: airHumidity,
            imageData: imageData,
            imageMimeType: image.ContentType,
            phenologicalPhase: phenologicalPhase
        );

        // Validar el tipo MIME
        if (!image.ContentType.StartsWith("image/"))
        {
            return BadRequest(new { errors = new { ImageMimeType = new[] { "The file must be an image" } } });
        }

        await _auditTrailRepository.AddAsync(auditTrail);
        return Ok(auditTrail);
    }

    [HttpGet("report")]
    public async Task<IActionResult> GenerateReport(int sowingId)
    {
        var pdfBytes = await _auditTrailCommandService.Handle(new GenerateAuditReportCommand(sowingId));
        return File(pdfBytes, "application/pdf", $"audit-trail-report-{sowingId}.pdf");
    }

    [HttpGet("{auditId}/image")]
    public async Task<IActionResult> GetAuditImage(int sowingId, int auditId)
    {
        var audit = await _auditTrailRepository.FindByIdAsync(auditId);

        if (audit == null)
            return NotFound(new { message = "Audit record not found" });

        if (audit.SowingId != sowingId)
            return BadRequest(new { message = "Audit does not belong to the provided sowing" });

        if (audit.ImageData == null || audit.ImageData.Length == 0)
            return NotFound(new { message = "Image not found for this audit" });

        return File(audit.ImageData, audit.ImageMimeType ?? "application/octet-stream", $"audit_{auditId}");
    }
}