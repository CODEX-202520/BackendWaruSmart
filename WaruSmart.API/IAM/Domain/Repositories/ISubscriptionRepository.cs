using WaruSmart.API.IAM.Domain.Model.Aggregates;

namespace WaruSmart.API.IAM.Domain.Repositories;

public interface ISubscriptionRepository
{
    Task<Subscription?> FindByIdAsync(int id);
    Task<IEnumerable<Subscription>> ListAsync();
    Task AddAsync(Subscription subscription);
    void Update(Subscription subscription);
    void Remove(Subscription subscription);
    Task<IEnumerable<Subscription>> FindAllActiveAsync();
}