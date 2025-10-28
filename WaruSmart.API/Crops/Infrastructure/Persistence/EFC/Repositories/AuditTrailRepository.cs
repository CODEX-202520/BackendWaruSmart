using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;
using WaruSmart.API.Crops.Domain.Model.Aggregates;
using WaruSmart.API.Crops.Domain.Repositories;
using WaruSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using WaruSmart.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace WaruSmart.API.Crops.Infrastructure.Persistence.EFC.Repositories
{
    public class AuditTrailRepository : BaseRepository<AuditTrail>, IAuditTrailRepository
    {
        private readonly AppDbContext _context;

        public AuditTrailRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuditTrail>> ListBySowingIdAsync(int sowingId)
        {
            return await _context.AuditTrails
                .Where(a => a.SowingId == sowingId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public new async Task<AuditTrail> AddAsync(AuditTrail auditTrail)
        {
            await _context.AuditTrails.AddAsync(auditTrail);
            await _context.SaveChangesAsync();
            return auditTrail;
        }

        public new async Task<AuditTrail> FindByIdAsync(int id)
        {
            return await _context.AuditTrails.FindAsync(id);
        }

        public async Task<byte[]> GenerateAuditReportAsync(int sowingId)
        {
            var auditTrails = await ListBySowingIdAsync(sowingId);
            var sowing = await _context.Sowings
                .Include(s => s.Crop)
                .FirstOrDefaultAsync(s => s.Id == sowingId);

            if (sowing == null)
                throw new KeyNotFoundException($"Sowing with ID {sowingId} not found");

            using (var stream = new System.IO.MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                document.Add(new Paragraph($"Audit Report for {sowing.Crop.Name}"));
                document.Add(new Paragraph($"Start Date: {sowing.StartDate:d}"));
                document.Add(new Paragraph($"Area: {sowing.AreaLand} m²"));
                document.Add(new Paragraph($"Current Phase: {sowing.PhenologicalPhase}"));
                document.Add(new Paragraph("\nAudit Trail Records:"));

                foreach (var audit in auditTrails)
                {
                    document.Add(new Paragraph("-------------------"));
                    document.Add(new Paragraph($"Date: {audit.CreatedAt}"));
                    document.Add(new Paragraph($"Phase: {audit.PhenologicalPhase}"));
                    document.Add(new Paragraph($"Description: {audit.Description}"));
                    document.Add(new Paragraph($"Soil Moisture: {audit.SoilMoisture}%"));
                    document.Add(new Paragraph($"Air Temperature: {audit.AirTemperature}°C"));
                    document.Add(new Paragraph($"Air Humidity: {audit.AirHumidity}%"));
                    document.Add(new Paragraph($"Soil Temperature: {audit.SoilTemperature}°C"));
                }

                document.Close();
                return stream.ToArray();
            }
        }
    }
}