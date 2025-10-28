using Microsoft.AspNetCore.Mvc;
using WaruSmart.API.Crops.Application.CommandServices;
using WaruSmart.API.Crops.Domain.Model.Commands;
using CreateAuditTrailCommand = WaruSmart.API.Crops.Application.CommandServices.CreateAuditTrailCommand;

namespace WaruSmart.API.Crops.Interfaces.REST;

[ApiController]
[Route("api/v1/crops/sowing/{sowingId}/audit-trail")]
public class AuditTrailController : ControllerBase
{
    private readonly IAuditTrailCommandService _auditTrailCommandService;

    public AuditTrailController(IAuditTrailCommandService auditTrailCommandService)
    {
        _auditTrailCommandService = auditTrailCommandService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAudit(int sowingId, [FromBody] CreateAuditTrailCommand command)
    {
        if (command.SowingId != sowingId) return BadRequest();
        var result = await _auditTrailCommandService.Handle(command);
        return Ok(result);
    }

    [HttpGet("report")]
    public async Task<IActionResult> GenerateReport(int sowingId)
    {
        var pdfBytes = await _auditTrailCommandService.Handle(new GenerateAuditReportCommand(sowingId));
        return File(pdfBytes, "application/pdf", $"audit-trail-report-{sowingId}.pdf");
    }
}