using WaruSmart.API.IAM.Domain.Model.Commands;
using WaruSmart.API.IAM.Domain.Model.Aggregates;

namespace WaruSmart.API.IAM.Domain.Services;

public interface ISubscriptionCommandService
{
    Task<Subscription> Handle(CreateSubscriptionCommand command);
    Task<Subscription> Handle(UpdateSubscriptionCommand command);
    Task Handle(int subscriptionId);  // For deletion
}