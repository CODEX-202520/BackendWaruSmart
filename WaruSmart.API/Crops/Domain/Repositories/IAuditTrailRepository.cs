using WaruSmart.API.Crops.Domain.Model.Aggregates;

namespace WaruSmart.API.Crops.Domain.Repositories;

public interface IAuditTrailRepository
{
    Task<IEnumerable<AuditTrail>> ListBySowingIdAsync(int sowingId);
    Task<AuditTrail> AddAsync(AuditTrail auditTrail);
    Task<AuditTrail> FindByIdAsync(int id);
}