using WaruSmart.API.IAM.Domain.Model.Aggregates;

namespace WaruSmart.API.IAM.Domain.Services;

public interface ISubscriptionQueryService
{
    Task<IEnumerable<Subscription>> ListAsync();
    Task<IEnumerable<Subscription>> ListActiveAsync();
    Task<Subscription?> FindByIdAsync(int id);
}