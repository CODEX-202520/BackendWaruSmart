using WaruSmart.API.Profiles.Domain.Model.Aggregates;
using WaruSmart.API.Shared.Domain.Repositories;

namespace WaruSmart.API.Profiles.Domain.Repositories;

public interface ISubscriptionRepository: IBaseRepository<Subscription>
{
    Task<IEnumerable<Subscription>> FindAllActiveAsync();
}