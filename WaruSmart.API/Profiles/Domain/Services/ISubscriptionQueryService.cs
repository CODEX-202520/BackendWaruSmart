using WaruSmart.API.Profiles.Domain.Model.Aggregates;
using WaruSmart.API.Profiles.Domain.Model.Queries;

namespace WaruSmart.API.Profiles.Domain.Services;

public interface ISubscriptionQueryService
{
    Task<IEnumerable<Subscription>> Handle(GetAllSubscriptionsQuery query);
    Task<Subscription?> Handle(GetSubscriptionByIdQuery query);
}
