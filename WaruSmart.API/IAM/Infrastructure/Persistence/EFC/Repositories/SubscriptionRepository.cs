using Microsoft.EntityFrameworkCore;
using WaruSmart.API.IAM.Domain.Model.Aggregates;
using WaruSmart.API.IAM.Domain.Repositories;
using WaruSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using WaruSmart.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace WaruSmart.API.IAM.Infrastructure.Persistence.EFC.Repositories;

public class SubscriptionRepository(AppDbContext context) 
    : BaseRepository<Subscription>(context), ISubscriptionRepository
{
    public async Task<IEnumerable<Subscription>> FindAllActiveAsync()
    {
        return await Context.Set<Subscription>()
            .Where(s => s.IsActive)
            .ToListAsync();
    }
}