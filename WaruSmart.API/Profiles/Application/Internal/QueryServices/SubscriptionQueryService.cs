using WaruSmart.API.Profiles.Domain.Model.Aggregates;
using WaruSmart.API.Profiles.Domain.Model.Queries;
using WaruSmart.API.Profiles.Domain.Repositories;
using WaruSmart.API.Profiles.Domain.Services;

namespace WaruSmart.API.Profiles.Application.Internal.QueryServices;

public class SubscriptionQueryService : ISubscriptionQueryService
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public SubscriptionQueryService(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<IEnumerable<Subscription>> Handle(GetAllSubscriptionsQuery query)
    {
        return await _subscriptionRepository.ListAsync();
    }

    public async Task<Subscription?> Handle(GetSubscriptionByIdQuery query)
    {
        return await _subscriptionRepository.FindByIdAsync(query.Id);
    }
}
