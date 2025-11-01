using WaruSmart.API.Shared.Domain.Repositories;
using WaruSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace WaruSmart.API.Shared.Infrastructure.Persistence.EFC.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task CompleteAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException dbEx)
        {
            // Collect entity types involved (if available) to help debugging
            var entryTypes = dbEx.Entries?.Select(e => e.Entity?.GetType().FullName ?? "<unknown>").Distinct();
            var entriesInfo = entryTypes is not null ? string.Join(", ", entryTypes) : "<no entries>";
            throw new Exception($"Could not save changes. Entities involved: {entriesInfo}. Exception: {dbEx}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not save changes. Exception: {ex}", ex);
        }
    }
}