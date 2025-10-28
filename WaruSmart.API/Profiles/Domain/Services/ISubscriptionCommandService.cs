using WaruSmart.API.Profiles.Domain.Model.Aggregates;
using WaruSmart.API.Profiles.Domain.Model.Commands;

namespace WaruSmart.API.Profiles.Domain.Services;

public interface ISubscriptionCommandService
{
    Task<Subscription?> Handle(CreateSubscriptionCommand command);
    Task<Subscription?> Handle(UpdateSubscriptionCommand command);
    Task<bool> Handle(DeleteSubscriptionCommand command);
}
