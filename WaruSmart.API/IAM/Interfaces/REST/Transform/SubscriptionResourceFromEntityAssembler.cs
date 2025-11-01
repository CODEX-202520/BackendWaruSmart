using WaruSmart.API.IAM.Domain.Model.Aggregates;
using WaruSmart.API.IAM.Interfaces.REST.Resources;

namespace WaruSmart.API.IAM.Interfaces.REST.Transform;

public class SubscriptionResourceFromEntityAssembler
{
    public SubscriptionResource ToResource(Subscription entity) =>
        new(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Price,
            entity.DurationInDays,
            entity.IsActive
        );
}