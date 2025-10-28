using WaruSmart.API.Profiles.Domain.Model.Commands;
using WaruSmart.API.Profiles.Interfaces.REST.Resources;

namespace WaruSmart.API.Profiles.Interfaces.REST.Transform;

public static class UpdateSubscriptionCommandFromResourceAssembler
{
    public static UpdateSubscriptionCommand ToCommandFromResource(int id, UpdateSubscriptionResource resource)
    {
        return new UpdateSubscriptionCommand(
            id,
            resource.Name,
            resource.Description,
            resource.Price,
            resource.DurationInDays
        );
    }
}
