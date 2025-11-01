using WaruSmart.API.IAM.Domain.Model.Commands;
using WaruSmart.API.IAM.Interfaces.REST.Resources;

namespace WaruSmart.API.IAM.Interfaces.REST.Transform;

public class UpdateSubscriptionCommandFromResourceAssembler
{
    public UpdateSubscriptionCommand ToCommand(int id, UpdateSubscriptionResource resource) =>
        new(
            id,
            resource.Name,
            resource.Description,
            resource.Price,
            resource.DurationInDays
        );
}