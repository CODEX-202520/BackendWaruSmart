using WaruSmart.API.Profiles.Domain.Model.Aggregates;
using WaruSmart.API.Profiles.Interfaces.REST.Resources;

namespace WaruSmart.API.Profiles.Interfaces.REST.Transform;

public static class SubscriptionResourceFromEntityAssembler
{
    public static SubscriptionResource ToResourceFromEntity(Subscription entity)
    {
        return new SubscriptionResource(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Price,
            entity.DurationInDays,
            entity.IsActive
        );
    }
}
