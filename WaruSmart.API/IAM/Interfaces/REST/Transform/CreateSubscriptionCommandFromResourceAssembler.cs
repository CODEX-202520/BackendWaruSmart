using WaruSmart.API.IAM.Domain.Model.Commands;
using WaruSmart.API.IAM.Interfaces.REST.Resources;

namespace WaruSmart.API.IAM.Interfaces.REST.Transform;

public class CreateSubscriptionCommandFromResourceAssembler
{
    public CreateSubscriptionCommand ToCommand(CreateSubscriptionResource resource) =>
        new(
            resource.Name,
            resource.Description,
            resource.Price,
            resource.DurationInDays
        );
}