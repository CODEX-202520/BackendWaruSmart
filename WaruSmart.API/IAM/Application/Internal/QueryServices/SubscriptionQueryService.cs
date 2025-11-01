using WaruSmart.API.IAM.Domain.Model.Aggregates;
using WaruSmart.API.IAM.Domain.Repositories;
using WaruSmart.API.IAM.Domain.Services;

namespace WaruSmart.API.IAM.Application.Internal.QueryServices;

public class SubscriptionQueryService : ISubscriptionQueryService
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public SubscriptionQueryService(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<IEnumerable<Subscription>> ListAsync()
    {
        return await _subscriptionRepository.ListAsync();
    }

    public async Task<IEnumerable<Subscription>> ListActiveAsync()
    {
        return await _subscriptionRepository.FindAllActiveAsync();
    }

    public async Task<Subscription?> FindByIdAsync(int id)
    {
        return await _subscriptionRepository.FindByIdAsync(id);
    }
}